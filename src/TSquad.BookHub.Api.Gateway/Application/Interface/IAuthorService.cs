using TSquad.BookHub.Api.Gateway.Application.RemoteDto;

namespace TSquad.BookHub.Api.Gateway.Application.Interface;

public interface IAuthorService
{
    Task<(bool IsSucces, AuthorRemoteDto? Author, string? ErrorMessage)> GetAuthor(string authorId);
}