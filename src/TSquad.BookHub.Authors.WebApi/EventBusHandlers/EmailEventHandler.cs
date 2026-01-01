using TSquad.BookHub.RabbitMQ.Bus.EventBus;
using TSquad.BookHub.RabbitMQ.Bus.EventQueue;

namespace TSquad.BookHub.Authors.WebApi.EventBusHandlers;

public class EmailEventHandler : IEventHandler<EmailEventQueue>
{
    private readonly ILogger<EmailEventHandler> _logger;

    public EmailEventHandler(ILogger<EmailEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(EmailEventQueue @event)
    {
        _logger.LogInformation("\n\n\n*********\neventData {EventTitle}\n*********\n\n\n", @event.Title);
        return Task.CompletedTask;
    }
}