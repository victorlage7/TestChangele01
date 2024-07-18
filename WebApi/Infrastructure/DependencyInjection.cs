using WebApi.Domain.Interfaces.Repositories;
using WebApi.Infrastructure.Repositories;

namespace WebApi.Infrastructure;

public static class DependencyInjection
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}