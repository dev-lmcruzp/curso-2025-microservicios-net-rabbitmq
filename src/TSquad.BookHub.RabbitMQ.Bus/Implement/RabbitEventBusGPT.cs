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

public class RabbitEventBusGpt : IEventBus, IAsyncDisposable
{
    private readonly IMediator _mediator;
    private readonly IServiceProvider _serviceProvider;

    private IConnection? _connection;

    private readonly Dictionary<string, List<Type>> _handlers = new();
    private readonly List<Type> _eventTypes = new();

    public RabbitEventBusGpt(
        IMediator mediator,
        IServiceProvider serviceProvider)
    {
        _mediator = mediator;
        _serviceProvider = serviceProvider;
    }

    // --------------------------------------------------
    // INITIALIZATION (ASYNC)
    // --------------------------------------------------
    public async Task InitializeAsync()
    {
        if (_connection != null)
            return;

        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
        };

        _connection = await factory.CreateConnectionAsync();
    }

    // --------------------------------------------------
    // COMMANDS
    // --------------------------------------------------
    public Task SendCommand<TCommand>(TCommand command)
        where TCommand : Command
        => _mediator.Send(command);

    // --------------------------------------------------
    // PUBLISH
    // --------------------------------------------------
    public async Task Publish<TEvent>(TEvent @event)
        where TEvent : Event
    {
        EnsureInitialized();

        var eventName = typeof(TEvent).Name;
        var channel = await _connection!.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: eventName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        var body = Encoding.UTF8.GetBytes(
            JsonConvert.SerializeObject(@event));

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: eventName,
            body: body);

        await channel.CloseAsync();
    }

    // --------------------------------------------------
    // SUBSCRIBE
    // --------------------------------------------------
    public async Task Subscribe<T, TH>()
        where T : Event
        where TH : IEventHandler<T>
    {
        EnsureInitialized();

        var eventName = typeof(T).Name;
        var handlerType = typeof(TH);

        if (!_eventTypes.Contains(typeof(T)))
            _eventTypes.Add(typeof(T));

        if (!_handlers.ContainsKey(eventName))
            _handlers[eventName] = new List<Type>();

        if (_handlers[eventName].Contains(handlerType))
            throw new ArgumentException(
                $"{handlerType.Name} already registered");

        _handlers[eventName].Add(handlerType);

        var channel = await _connection!.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: eventName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        await channel.BasicQosAsync(0, 1, false);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());

            try
            {
                using var scope = _serviceProvider.CreateScope();

                var eventType = _eventTypes
                    .Single(t => t.Name == eventName);

                var @event = JsonConvert
                    .DeserializeObject(message, eventType)!;

                foreach (var handler in _handlers[eventName])
                {
                    var instance = scope.ServiceProvider
                        .GetRequiredService(handler);

                    var concreteType = typeof(IEventHandler<>)
                        .MakeGenericType(eventType);

                    await (Task)concreteType
                        .GetMethod("HandleAsync")!
                        .Invoke(instance, new[] { @event })!;
                }

                await channel.BasicAckAsync(
                    ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                await channel.BasicNackAsync(
                    ea.DeliveryTag, false, true);
            }
        };

        await channel.BasicConsumeAsync(
            queue: eventName,
            autoAck: false,
            consumer: consumer);
    }

    // --------------------------------------------------
    // DISPOSE
    // --------------------------------------------------
    public async ValueTask DisposeAsync()
    {
        if (_connection is { IsOpen: true })
            await _connection.CloseAsync();

        _connection?.Dispose();
    }
    
    private void EnsureInitialized()
    {
        if (_connection == null)
            throw new InvalidOperationException(
                "RabbitEventBus not initialized. Call InitializeAsync() first.");
    }
}

// En Program.cs / Startup
// builder.Services.AddSingleton<IEventBus, RabbitEventBus>();

// var eventBus = app.Services.GetRequiredService<RabbitEventBus>();
// await eventBus.InitializeAsync();
// await eventBus.Subscribe<OrderCreatedEvent, OrderCreatedHandler>();
