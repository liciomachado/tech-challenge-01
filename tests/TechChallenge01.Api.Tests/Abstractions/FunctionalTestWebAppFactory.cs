using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechChallenge01.Infra.Data.Context;
using Testcontainers.PostgreSql;

namespace TechChallenge01.Api.Tests.Abstractions;

public class FunctionalTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //var contextOptions = services.SingleOrDefault(
            //    d => d.ServiceType == typeof(DbContextOptions<DataContext>));

            services.Remove(services.Single(a => typeof(DbContextOptions<DataContext>) == a.ServiceType));
            services.AddDbContext<DataContext>(options => options
                .UseNpgsql(_postgreSqlContainer.GetConnectionString()));

            //services.AddDbContext<DataContext>(options =>
            //{
            //    options.UseInMemoryDatabase("_dataContextTest")
            //    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            //});
            RunScriptDatabase(services);
        });
    }

    private void RunScriptDatabase(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var dataContext = serviceProvider.GetRequiredService<DataContext>();
        dataContext.Database.EnsureCreated();
    }
    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _postgreSqlContainer.StopAsync();
    }
}
