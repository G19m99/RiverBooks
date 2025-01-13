using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using RiverBooks.Books.Data;

namespace RiverBooks.Books;


public static class BookServiceExtensions
{
    public static IServiceCollection AddBooksServices(this IServiceCollection services, ConfigurationManager config, ILogger logger)
    {
        string? connectionString = config.GetConnectionString("BooksConnectionString");
        Console.WriteLine("Connection" + connectionString);
        services.AddDbContext<BookDbContext>(options => 
            options.UseSqlServer(connectionString));

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBookeService, BookService>();

        logger.Information("{Module} module services registered", "Books");

        return services;
    }
}