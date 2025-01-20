using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using RiverBooks.Books.Data;
using System.Reflection;

namespace RiverBooks.Books;


public static class BookServiceExtensions
{
    public static IServiceCollection AddBooksModuleServices(this IServiceCollection services, ConfigurationManager config, ILogger logger, List<Assembly> mediatRAssemblies)
    {
        string? connectionString = config.GetConnectionString("BooksConnectionString");
        Console.WriteLine("Connection" + connectionString);
        services.AddDbContext<BookDbContext>(options => 
            options.UseSqlServer(connectionString));

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBookeService, BookService>();

        //if MediatoR is needed in this module register self to list of MediatoR assemblies
        mediatRAssemblies.Add(typeof(BookServiceExtensions).Assembly);

        logger.Information("{Module} module services registered", "Books");

        return services;
    }
}