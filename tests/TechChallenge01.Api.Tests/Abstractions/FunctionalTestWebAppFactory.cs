using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using TechChallenge01.Infra.Data;

namespace TechChallenge01.Api.Tests.Abstractions;

public class FunctionalTestWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var contextOptions = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DataContext>));

            services.Remove(contextOptions);
            services.AddDbContext<DataContext>(options =>
            {
                options.UseInMemoryDatabase("_dataContextTest")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
        });
    }
}
