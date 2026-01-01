using System.Diagnostics;
using System.Text.Json;
using TSquad.BookHub.Api.Gateway.Application.Interface;
using TSquad.BookHub.Api.Gateway.Application.RemoteDto;

namespace TSquad.BookHub.Api.Gateway.MessageHandler;

public class BookHandler : DelegatingHandler
{

    private readonly ILogger<Boolean> _logger;
    private readonly IAuthorService _authorService;
    public BookHandler(ILogger<Boolean> logger, IAuthorService authorService)
    {
        _logger = logger;
        _authorService = authorService;
    }
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, 
        CancellationToken token)
    {
        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation("Inicia el request");
        var response =  await base.SendAsync(request, token);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(token);
            var opts = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true 
            };
            var result = JsonSerializer.Deserialize<BookRemoteDto>(content, opts);
            
            var authorResponse = await _authorService.GetAuthor(result!.AuthorId);
            if (authorResponse.IsSucces)
                result.Author = authorResponse.Author!;
            
            var resultStr = JsonSerializer.Serialize(result);
            response.Content = new StringContent(resultStr, System.Text.Encoding.UTF8, "application/json");
        }
        
        stopwatch.Stop();
        _logger.LogInformation($"Este proceso se hizo en {stopwatch.ElapsedMilliseconds} ms");
        return response;
    }
}