using TSquad.BookHub.RabbitMQ.Bus.Events;

namespace TSquad.BookHub.RabbitMQ.Bus.EventBus;

public interface IEventHandler
{
}

public interface IEventHandler<in TEvent> : IEventHandler where TEvent : Event
{
    Task HandleAsync(TEvent @event);
}