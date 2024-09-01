using Aram.BFF.Api.Base;
using Aram.BFF.Infrastructure.Common.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Aram.BFF.Api.IntegrationTests.Common;

public class ApplicationApiFactory : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
{
    private SqlServerTestDatabase _testDatabase = null!;

    public HttpClient HttpClient { get; private set; } = null!;

    public Task InitializeAsync()
    {
        HttpClient = CreateClient();

        return Task.CompletedTask;
    }

    public new Task DisposeAsync()
    {
        _testDatabase.Dispose();

        return Task.CompletedTask;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _testDatabase = SqlServerTestDatabase.CreateAndInitialize();

        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<DbContextOptions<ApplicationDbContext>>()
                .AddDbContext<ApplicationDbContext>((sp, options) => options.UseSqlServer(_testDatabase.Connection));
        });
    }

    public void ResetDatabase()
    {
        _testDatabase.ResetDatabase();
    }
}