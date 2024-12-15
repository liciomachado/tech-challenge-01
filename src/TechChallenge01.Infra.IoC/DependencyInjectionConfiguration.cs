 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.UseCases;
using TechChallenge01.Domain.Interfaces;
using TechChallenge01.Infra.Data.Context;
using TechChallenge01.Infra.Data.Repositories;
using MassTransit;
using TechChallenge01.Application.Consumers;




namespace TechChallenge01.Infra.IoC;

public static class DependencyInjectionConfiguration
{
    public static void AddInjections(this IServiceCollection services, IConfiguration configuration)
    {
        //Data
        services.AddDbContext<DataContext>(options => options
            .UseNpgsql(configuration.GetConnectionString("postgres")));

        //Repo
        services.AddScoped<IContactRepository, ContactRepository>();

        //Services
        services.AddScoped<IInsertContactUseCase, InsertContactUseCase>();
        services.AddScoped<IDeleteContactsUseCase, DeleteContactUseCase>();
        services.AddScoped<IUpdateContactUseCase, UpdateContactUseCase>();
        services.AddScoped<IInsertContactUseCaseV2, InsertContactUseCaseV2>();
        services.AddScoped<IDeleteContactsUseCaseV2, DeleteContactUseCaseV2>();
        services.AddScoped<IUpdateContactUseCaseV2, UpdateContactUseCaseV2>();
        services.AddScoped<IGetContactsUseCase, GetContactsUseCase>();
        services.AddScoped<IContactPublisher, ContactPublisher>();
        const string serviceName = "MyService";

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

                cfg.ConfigureEndpoints(context);
            });
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
