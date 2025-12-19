using System.Text.Json;
using TSquad.BookHub.ShoppingCart.WebApi.Application.Dto.Remote;
using TSquad.BookHub.ShoppingCart.WebApi.Application.Interface.Infrastructure.Services;

namespace TSquad.BookHub.ShoppingCart.WebApi.Infrastructure.Infrastructure.Services;

public class BookService : IBookService
{
    private readonly ILogger<BookService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public BookService(IHttpClientFactory httpClientFactory, ILogger<BookService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<(bool result, RemoteBookDto? book, string? error)> GetBook(string bookId, CancellationToken cancellationToken)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("BooksApi");
            var response = await client.GetAsync($"/api/books/{bookId}", cancellationToken);
            
            if (!response.IsSuccessStatusCode)
                return (false, null, response.ReasonPhrase);
            
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<RemoteBookDto>(content, options);
            return (true, result, null);
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return (false, null, e.Message);
        }
    }
}