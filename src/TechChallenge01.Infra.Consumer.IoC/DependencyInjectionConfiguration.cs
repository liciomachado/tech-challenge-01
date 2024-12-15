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
        //Data
        services.AddDbContext<DataContext>(options => options
            .UseNpgsql(configuration.GetConnectionString("postgres")));

        //Repo
        services.AddScoped<IContactRepository, ContactRepository>();

        const string serviceName = "InfraConsumer";

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {

                var rabbitMqHost = configuration["RabbitMQ:RABBITMQ_HOST"];
                var rabbitMqUser = configuration["RabbitMQ:RABBITMQ_USER"];
                var rabbitMqPassword = configuration["RabbitMQ:RABBITMQ_PASSWORD"];

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

                // Configuração de fila específica para DeleteContactConsumer
                cfg.ReceiveEndpoint("delete-contact-queue", e =>
                {
                    e.ConfigureConsumer<DeleteContactConsumer>(context);
                });

                cfg.ConfigureEndpoints(context);
            });

            x.AddConsumer<InsertContactConsumer>();
            x.AddConsumer<UpdateContactConsumer>();
            x.AddConsumer<DeleteContactConsumer>();
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
