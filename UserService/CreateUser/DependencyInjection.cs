using Messaging;
using Messaging.Interface;

namespace CreateUser
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMqService, RabbitMqService>();

    }
}
}
