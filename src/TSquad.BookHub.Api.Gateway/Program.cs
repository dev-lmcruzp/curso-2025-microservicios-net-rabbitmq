using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using TSquad.BookHub.Api.Gateway.ExtensionInjections;
using TSquad.BookHub.Api.Gateway.MessageHandler;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddInfrastructureConfigureService();

builder.Services.AddHttpClient("AuthorService", config =>
{
    config.BaseAddress = new Uri(builder.Configuration["Services:Authors"]!);
});

builder.Services.AddOcelot(builder.Configuration)
    .AddDelegatingHandler<BookHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
await app.UseOcelot();

app.Run();


// Rabbit MQ

// docker run -it --rm -d --hostname my-rabbit-server --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:4-management
// docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:4-management
