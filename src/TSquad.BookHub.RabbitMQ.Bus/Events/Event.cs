namespace TSquad.BookHub.RabbitMQ.Bus.Events;

public abstract class Event
{
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}