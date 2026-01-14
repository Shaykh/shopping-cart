using Basket.Application.Common.Interfaces;
using Basket.Application.Repositories;
using Basket.Infrastructure.Configuration;
using Basket.Infrastructure.Repositories;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Basket.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register Redis
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var cacheSettings = configuration.GetSection(CacheSettings.SectionName).Get<CacheSettings>()
                ?? throw new InvalidOperationException("CacheSettings configuration is missing.");

            return ConnectionMultiplexer.Connect(cacheSettings.ConnectionString);
        });

        // Register Repository
        services.AddScoped<IBasketRepository, BasketRepository>();

        // Register MassTransit with RabbitMQ
        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((ctx, cfg) =>
            {
                var eventBusSettings = configuration.GetSection(EventBusSettings.SectionName).Get<EventBusSettings>()
                    ?? throw new InvalidOperationException("EventBusSettings configuration is missing.");

                cfg.Host(eventBusSettings.HostAddress, h =>
                {
                    if (!string.IsNullOrEmpty(eventBusSettings.Username))
                    {
                        h.Username(eventBusSettings.Username);
                    }
                    if (!string.IsNullOrEmpty(eventBusSettings.Password))
                    {
                        h.Password(eventBusSettings.Password);
                    }
                });

                // Configure message serialization
                cfg.ConfigureJsonSerializerOptions(options =>
                {
                    options.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                    return options;
                });
            });
        });

        // Register EventBus
        services.AddScoped<IEventBus, EventBus.EventBus>();

        return services;
    }
}
