using MediatR;
using TSquad.BookHub.Authors.WebApi.Endpoints;
using TSquad.BookHub.Authors.WebApi.EventBusHandlers;
using TSquad.BookHub.Authors.WebApi.ExtensionInjections;
using TSquad.BookHub.Books.WebApi.ExtensionInjections;
using TSquad.BookHub.RabbitMQ.Bus.EventBus;
using TSquad.BookHub.RabbitMQ.Bus.EventQueue;
using TSquad.BookHub.RabbitMQ.Bus.Implement;

var builder = WebApplication.CreateBuilder(args);

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.AddJsonFile($"appsettings.{env}.json", optional: true);

// builder.Services.AddTransient<IEventBus, RabbitEventBus>();
builder.Services.AddSingleton<IEventBus, RabbitEventBus>(sp =>
{
    var scf = sp.GetRequiredService<IServiceScopeFactory>();
    return new RabbitEventBus(sp.GetService<IMediator>()!, scf);
});

builder.Services.AddTransient<EmailEventHandler>();

builder.Services.AddTransient<IEventHandler<EmailEventQueue>, EmailEventHandler>();
// Application Layer
builder.Services.AddApplicationConfigureServices();

// Infrastructure Layer
builder.Services.AddPersistenceConfigureServices(builder.Configuration);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapAuthorsEndpoints();

await app.Services.InitialiseDataBaseAsync();

var eventBus = app.Services.GetRequiredService<IEventBus>();
await eventBus.Subscribe<EmailEventQueue, EmailEventHandler>();

app.Run();