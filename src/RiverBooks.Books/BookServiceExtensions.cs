using Microsoft.Extensions.DependencyInjection;

namespace RiverBooks.Books;


public static class BookServiceExtensions
{
    public static IServiceCollection AddBooksServices(this IServiceCollection services)
    {
        services.AddScoped<IBookeService, BookService>();

        return services;
    }
}