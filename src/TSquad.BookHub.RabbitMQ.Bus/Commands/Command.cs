using TSquad.BookHub.RabbitMQ.Bus.Events;

namespace TSquad.BookHub.RabbitMQ.Bus.Commands;

public abstract class Command : Message
{
    public DateTime Timestamp { get; protected set; } = DateTime.UtcNow;
}