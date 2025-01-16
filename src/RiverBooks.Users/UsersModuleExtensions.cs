using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiverBooks.Users;
using RiverBooks.Users.Data;
using Serilog;
using System.Reflection;

public static class UsersModuleExtensions
{
    public static IServiceCollection AddUsersModuleServices(this IServiceCollection services, ConfigurationManager config, ILogger logger, List<Assembly> mediatRAssemblies)
    {
        string connStr = config.GetConnectionString("UsersConnectionString")!;

        services.AddDbContext<UsersDbContext>(opt => opt.UseSqlServer(connStr));
        services.AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<UsersDbContext>();

        //user services
        services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

        //if MediatoR is needed in this module register self to list of MediatoR assemblies
        mediatRAssemblies.Add(typeof(UsersModuleExtensions).Assembly);

        logger.Information("{Module} module services registered", "Users");

        return services;
    }
}
