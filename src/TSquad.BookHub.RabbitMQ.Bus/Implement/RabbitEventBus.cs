using System.Text;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TSquad.BookHub.RabbitMQ.Bus.Commands;
using TSquad.BookHub.RabbitMQ.Bus.EventBus;
using TSquad.BookHub.RabbitMQ.Bus.Events;

namespace TSquad.BookHub.RabbitMQ.Bus.Implement;

public class RabbitEventBus : IEventBus
{
    private readonly IMediator _mediator;
    private readonly Dictionary<string, List<Type>> _handlers;
    private readonly List<Type> _eventTypes;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitEventBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
    {
        _mediator = mediator;
        _serviceScopeFactory = serviceScopeFactory;
        _handlers = new Dictionary<string, List<Type>>();
        _eventTypes = [];
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task SendCommand<TCommand>(TCommand @event) where TCommand : Command => _mediator.Send(@event);

    public async Task Publish<TEvent>(TEvent @event) where TEvent : Event
    {
        var factory = new ConnectionFactory()
        {
            HostName = "rabbitmq",
            UserName = "guest",
            Password = "guest",
        };

        await using var conn = await factory.CreateConnectionAsync();
        await using var channel = await conn.CreateChannelAsync();
        // var conn = await factory.CreateConnectionAsync();
        // var channel = await conn.CreateChannelAsync();

        var eventName = @event.GetType().Name;
        await channel.QueueDeclareAsync(eventName, false, false, false);
        var message = JsonConvert.SerializeObject(@event);

        var body = Encoding.UTF8.GetBytes(message);
        // var props = new BasicProperties();
        // await channel.BasicPublishAsync("", eventName, false, props, body);
        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: eventName, body: body);

        await channel.CloseAsync();
        await conn.CloseAsync();
    }

    public async Task Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>
    {
        var eventName = typeof(T).Name;
        var handlerEventType = typeof(TH);

        if (!_eventTypes.Contains(typeof(T)))
            _eventTypes.Add(typeof(T));

        if (!_handlers.ContainsKey(eventName))
            _handlers.Add(eventName, []);

        if (_handlers[eventName].Contains(handlerEventType))
            throw new ArgumentException($"{handlerEventType.Name} already registered");
        
        _handlers[eventName].Add(handlerEventType);

        var factory = new ConnectionFactory()
        {
            HostName = "rabbitmq", 
            UserName = "guest", 
            Password = "guest", 
        };

        // await using var connection = await factory.CreateConnectionAsync();
        // await using var channel = await connection.CreateChannelAsync();
        
        _connection ??= await factory.CreateConnectionAsync();
        _channel ??= await _connection.CreateChannelAsync();
        
        await _channel.QueueDeclareAsync(eventName, false, false, false);
        var consumer = new AsyncEventingBasicConsumer(_channel);
        
        consumer.ReceivedAsync += ConsumerDelegate;

        await _channel.BasicConsumeAsync(eventName, autoAck: true, consumer: consumer);
        
        // await channel.CloseAsync();
        // await connection.CloseAsync();
    }

    private async Task ConsumerDelegate(object sender, BasicDeliverEventArgs @event)
    {
        var eventName = @event.RoutingKey;
        var message = Encoding.UTF8.GetString(@event.Body.ToArray());

        try
        {
            if(!_handlers.TryGetValue(eventName, out var subscriptions))
                return;

            using var scope = _serviceScopeFactory.CreateScope();
            foreach (var subscription in subscriptions)
            {
                var handler = scope.ServiceProvider.GetService(subscription);
                // var handler = Activator.CreateInstance(subscription);
                if(handler is null) continue;

                var typeEvent = _eventTypes.SingleOrDefault(x => x.Name == eventName);
                    
                var dataEvent = JsonConvert.DeserializeObject(message, typeEvent!);
                    
                var concreteType = typeof(IEventHandler<>).MakeGenericType(typeEvent!);
                    
                await (Task)concreteType.GetMethod("HandleAsync")!.Invoke(handler, [dataEvent])!;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}