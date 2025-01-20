using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;

namespace RiverBooks.OrderProcessing;

public static class OrderProcessingModuleExtensions
{
    public static IServiceCollection AddOrderProcessingModuleServices(this IServiceCollection services, ConfigurationManager config, ILogger logger, List<Assembly> mediatRAssemblies)
    {
        string connStr = config.GetConnectionString("OrderProcessingConnectionString")!;

        services.AddDbContext<OrderProcessingDbContext>(opt => opt.UseSqlServer(connStr));

        services.AddScoped<IOrderRepository, EfOrderRepository>();

        //if MediatoR is needed in this module register self to list of MediatoR assemblies
        mediatRAssemblies.Add(typeof(OrderProcessingModuleExtensions).Assembly);

        logger.Information("{Module} module services registered", "OrderProcessing");

        return services;
    }
}
