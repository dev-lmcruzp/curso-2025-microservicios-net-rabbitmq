using System.Text.Json;
using TSquad.BookHub.Api.Gateway.Application.Interface;
using TSquad.BookHub.Api.Gateway.Application.RemoteDto;

namespace TSquad.BookHub.Api.Gateway.Infrastructure.Services;

public class AuthorService: IAuthorService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<AuthorService> _logger;

    public AuthorService(IHttpClientFactory httpClientFactory, ILogger<AuthorService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<(bool IsSucces, AuthorRemoteDto? Author, string? ErrorMessage)> GetAuthor(string authorId)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("AuthorService");
            var response = await client.GetAsync($"/authors/{authorId}");

            
            if (!response.IsSuccessStatusCode)
                return (false, null, response.ReasonPhrase);
            
            var content = await response.Content.ReadAsStringAsync();
            var opts = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true 
            };
            var result = JsonSerializer.Deserialize<AuthorRemoteDto>(content, opts);
            return (true, result, null);
            
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return (false, null, e.Message);
        }
    }
}