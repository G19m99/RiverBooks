﻿using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace RiverBooks.SharedKernel;

public static class BehaviorExtensions
{
    public static IServiceCollection AddMediatRLogginBehavior(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }
    /// <summary>
    /// don't forget to wire up the validatiors
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddMediatRValidationBehavior(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(FluentValidationBehavior<,>));
        return services;
    }

    public static IServiceCollection AddValidatorsFromAssemblyContaining<T>(
    this IServiceCollection services)
    {
        // Get the assembly containing the specified type
        var assembly = typeof(T).GetTypeInfo().Assembly;

        // Find all validator types in the assembly
        var validatorTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType &&
                          i.GetGenericTypeDefinition() == typeof(IValidator<>)))
            .ToList();

        // Register each validator with its implemented interfaces
        foreach (var validatorType in validatorTypes)
        {
            var implementedInterfaces = validatorType.GetInterfaces()
                .Where(i => i.IsGenericType &&
                          i.GetGenericTypeDefinition() == typeof(IValidator<>));

            foreach (var implementedInterface in implementedInterfaces)
            {
                services.AddTransient(implementedInterface, validatorType);
            }
        }

        return services;
    }
}
