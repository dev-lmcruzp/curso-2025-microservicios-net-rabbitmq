using TSquad.BookHub.ShoppingCart.WebApi.Endpoints;
using TSquad.BookHub.ShoppingCart.WebApi.ExtensionInjections;

var builder = WebApplication.CreateBuilder(args);

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.AddJsonFile($"appsettings.{env}.json", optional: true);

builder.Services.AddApplicationConfigureServices();
builder.Services.AddPersistenceConfigureServices(builder.Configuration);
builder.Services.AddInfrastructureConfigureService();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

Console.WriteLine("\n\n\n");
Console.WriteLine("*****************************");
Console.WriteLine(builder.Configuration["Services:Books"]!);
Console.WriteLine("*****************************");
Console.WriteLine("\n\n\n");
builder.Services.AddHttpClient("BooksApi", cfg =>
{
    cfg.BaseAddress = new Uri(builder.Configuration["Services:Books"]!);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapShoppingCartsEndpoints();

await app.Services.InitialiseDataBaseAsync();

app.Run();