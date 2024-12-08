 
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
        services.AddSingleton<IAuthenticationUseCase, AuthenticationUseCase>();
        services.AddScoped<IInsertContactUseCase, InsertContactUseCase>();
        services.AddScoped<IGetContactsUseCase, GetContactsUseCase>();
        services.AddScoped<IUpdateContactUseCase, UpdateContactUseCase>();
        services.AddScoped<IDeleteContactsUseCase, DeleteContactUseCase>();
        services.AddScoped<IContactPublisher, ContactPublisher>();
        const string serviceName = "MyService";

         
        services.AddMassTransit(x =>
        {
            x.AddConsumer<InsertContactConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("insert-contact-queue", e =>
                {
                    e.ConfigureConsumer<InsertContactConsumer>(context);
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
