using WebApi.Domain.Interfaces;
using WebApi.Domain.Interfaces.Services;
using WebApi.Domain.Services;

namespace WebApi.Domain;

public static class DependencyInjection
{
    /// <summary>
    /// Inclui as dependências do domínio.
    /// </summary>
    /// <param name="services">Interface IServiceCollection</param>
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IContactDomainService, ContactDomainDomainService>();
    }
}