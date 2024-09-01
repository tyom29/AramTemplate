using Aram.BFF.Application.Common.Interfaces;
using Aram.BFF.Application.Features.Sample.Interfaces;
using Aram.BFF.Infrastructure.Common.Persistence;
using Aram.BFF.Infrastructure.Repositories.Samples;
using Aram.BFF.Infrastructure.Services.Email;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Aram.BFF.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddServices(configuration)
            .AddPersistence();
    }

    private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();

        var emailServiceSettings = new EmailServiceSettings();
        configuration.Bind(EmailServiceSettings.Section, emailServiceSettings);

        services.AddSingleton(Options.Create(emailServiceSettings));

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer("connectionString"));


        services.AddScoped<ISampleRepository, SamplesRepository>();

        services.AddScoped<IUnitOfWork>(serviceProvider =>
            serviceProvider.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}