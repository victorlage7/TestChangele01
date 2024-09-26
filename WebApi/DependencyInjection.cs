using Messaging;
using Messaging.Interface;
using System.Configuration;
using WebApi.Interfaces;
using WebApi.Services;

namespace WebApi;

public static class DependencyInjection
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IContactAppService, ContactAppService>();

        services.AddSingleton<IRabbitMqService, RabbitMqService>();

    }
}