using TSquad.BookHub.Authors.WebApi.Endpoints;
using TSquad.BookHub.Authors.WebApi.ExtensionInjections;
using TSquad.BookHub.Books.WebApi.ExtensionInjections;

var builder = WebApplication.CreateBuilder(args);

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.AddJsonFile($"appsettings.{env}.json", optional: true);

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

app.Run();