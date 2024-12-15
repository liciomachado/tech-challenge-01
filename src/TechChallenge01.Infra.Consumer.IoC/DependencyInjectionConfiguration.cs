using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using TechChallenge01.Domain.Interfaces;
using MassTransit;
using TechChallenge01.Infra.Data.Context;
using TechChallenge01.Infra.Data.Repositories;
using TechChallenge01.Application.Consumers;

namespace TechChallenge01.Infra.Consumer.IoC;

public static class DependencyInjectionConfiguration
{
    public static void AddInjections(this IServiceCollection services, IConfiguration configuration)
    {
        // TODO: Usar em arquivo de configuração
        //Data
        services.AddDbContext<DataContext>(options => options
            .UseNpgsql("Server=db;Port=5432;Database=tech_challenge;User Id=admin;Password=admin;Include Error Detail=True;"));

        //Repo
        services.AddScoped<IContactRepository, ContactRepository>();

        const string serviceName = "InfraConsumer";

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {

                // TODO: Usar em arquivo de configuração
                var rabbitMqHost = "rabbitmq";
                var rabbitMqUser = "guest";
                var rabbitMqPassword = "guest";

                cfg.Host(rabbitMqHost, h =>
                {
                    h.Username(rabbitMqUser);
                    h.Password(rabbitMqPassword);
                });

                // Configuração de fila específica para InsertContactConsumer
                cfg.ReceiveEndpoint("insert-contact-queue", e =>
                {
                    e.ConfigureConsumer<InsertContactConsumer>(context);
                });

                // Configuração de fila específica para UpdateContactConsumer
                cfg.ReceiveEndpoint("update-contact-queue", e =>
                {
                    e.ConfigureConsumer<UpdateContactConsumer>(context);
                });

                cfg.ConfigureEndpoints(context);
            });

            x.AddConsumer<InsertContactConsumer>();
            x.AddConsumer<UpdateContactConsumer>();
        });

        services.AddOpenTelemetry()
        .ConfigureResource(resource => resource.AddService(serviceName))
        .WithTracing(tracing => tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
        )
        .WithMetrics(metrics => 
        {
            metrics
                .AddRuntimeInstrumentation()
                .AddProcessInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddPrometheusExporter();  // Exposição para Prometheus
        });
    }
}
