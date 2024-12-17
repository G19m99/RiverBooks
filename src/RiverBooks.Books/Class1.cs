using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace RiverBooks.Books;

internal interface IBookeService
{
    IEnumerable<BookDto> ListBooks();
}

internal record BookDto(Guid Id, string Title, string Author);

internal class BookService : IBookeService
{
    public IEnumerable<BookDto> ListBooks()
    {
        return [
            new BookDto(Guid.NewGuid(), "some test", ""),
            new BookDto(Guid.NewGuid(), "some test", ""),
            new BookDto(Guid.NewGuid(), "some test", "")
        ];
    }
}
public static class BookEndpoints
{
    public static void MapBookEndpoints(this WebApplication app)
    {
        app.MapGet("/books", (IBookeService bookService) =>
        {
            return bookService.ListBooks();
        });
    }
}

public static class BookServiceExtensions
{
    public static IServiceCollection AddBooksServices(this IServiceCollection services)
    {
        services.AddScoped<IBookeService, BookService>();

        return services;
    }
}