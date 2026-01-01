using TSquad.BookHub.RabbitMQ.Bus.Events;

namespace TSquad.BookHub.RabbitMQ.Bus.EventQueue;

public class EmailEventQueue : Event
{
    public string To { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }

    public EmailEventQueue(string to, string title, string body)
    {
        To = to;
        Title = title;
        Body = body;
    }
}