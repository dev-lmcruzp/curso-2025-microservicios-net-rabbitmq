using System.Linq.Expressions;
using AutoMapper;
using GenFu;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TSquad.BookHub.Books.WebApi.Application.Interface.Persistence;
using TSquad.BookHub.Books.WebApi.Application.UseCases.Books.Commands.CreateBookCommand;
using TSquad.BookHub.Books.WebApi.Application.UseCases.Books.Queries.GetAllBookQuery;
using TSquad.BookHub.Books.WebApi.Application.UseCases.Books.Queries.GetBookQuery;
using TSquad.BookHub.Books.WebApi.Domain.Entities;

namespace TSquad.BookHub.Books.Tests;

public class BookServiceTest
{
    private List<Book> GetDataTest()
    {
        A.Configure<Book>()
            .Fill(x => x.Title).AsArticleTitle()
            .Fill(x => x.ExternalBookId, () => Guid.NewGuid().ToString())
            .Fill(x => x.AuthorId, () => Guid.NewGuid().ToString());

        var result = A.ListOf<Book>(30);
        result[0].ExternalBookId = Guid.Empty.ToString();
        return result;
    }
    
    private Mock<IUnitOfWork> CreateUnitOfWork()
    {
        var moqBookReadRepository = new Mock<IReadRepository<Book, long>>();
        var moqBookWriteRepository = new Mock<IWriteRepository<Book>>();
        var moqUnitOfWork = new Mock<IUnitOfWork>();
        
        var dataTest = GetDataTest();
        
        moqBookReadRepository
            .Setup(r => r.GetAllAsync())
            .Returns(dataTest.AsQueryable());
        
        moqBookReadRepository
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<Book, bool>>>()))
            .ReturnsAsync(dataTest.FirstOrDefault());

        moqBookWriteRepository
            .Setup(r => r.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
            .Callback<Book, CancellationToken>((book, _) =>
            {
                book.Id = 123;
            })
            .Returns<Book, CancellationToken>((book, _) => Task.FromResult(book));
        
        moqUnitOfWork
            .Setup(u => u.BookReadRepository)
            .Returns(moqBookReadRepository.Object);
        
        moqUnitOfWork
            .Setup(u => u.BookWriteRepository)
            .Returns(moqBookWriteRepository.Object);
        
        return moqUnitOfWork;
        
    }
    
    [Fact]
    public async Task GetBooks()
    {
        var mapConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingTest());
        }, NullLoggerFactory.Instance);

        var mapper = mapConfig.CreateMapper();
        var moqUnitOfWork = CreateUnitOfWork();
        var handler = new GetAllBookHandler(moqUnitOfWork.Object, mapper);
        var query =  new GetAllBookQuery();
        
        var books = await handler.Handle(query, CancellationToken.None);
        
        Assert.NotEmpty(books);
    }
    
    [Fact]
    public async Task GetBook()
    {
        var mapConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingTest());
        }, NullLoggerFactory.Instance);

        var mapper = mapConfig.CreateMapper();
        var moqUnitOfWork = CreateUnitOfWork();
        var handler = new GetBookHandler(moqUnitOfWork.Object, mapper);
        var query =  new GetBookQuery(Guid.NewGuid().ToString());
        
        var book = await handler.Handle(query, CancellationToken.None);
        
        Assert.NotNull(book);
    }
    
    [Fact]
    public async Task Add()
    {
        var mapConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingTest());
        }, NullLoggerFactory.Instance);

        var mapper = mapConfig.CreateMapper();
        var moqUnitOfWork = CreateUnitOfWork();
        var handler = new CreateBookHandler(moqUnitOfWork.Object, mapper);
        var command = A.New<CreateBookCommand>();
        var book = await handler.Handle(command, CancellationToken.None);
        
        Assert.NotNull(book);
    }
}