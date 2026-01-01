using TSquad.BookHub.RabbitMQ.Bus.Commands;
using TSquad.BookHub.RabbitMQ.Bus.Events;

namespace TSquad.BookHub.RabbitMQ.Bus.EventBus;

public interface IEventBus
{
  Task SendCommand<TCommand>(TCommand @event) where TCommand : Command;
  Task Publish<TEvent>(TEvent @event) where TEvent : Event;

  Task Subscribe<T, TH>() where T : Event
      where TH : IEventHandler<T>;

}