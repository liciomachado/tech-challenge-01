 
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

        const string serviceName = "MyService";


       
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

        //// Configurar o Prometheus
        //services.AddOpenTelemetry()
        //      .WithTracing(tracerProviderBuilder =>
        //      {
        //          tracerProviderBuilder
        //              .AddAspNetCoreInstrumentation()
        //              .AddHttpClientInstrumentation()
        //              .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyService"));
        //          //.AddConsoleExporter(); // Opcional: Adiciona um exportador de traços ao console
        //      })
        //      .WithMetrics(metricsProviderBuilder =>
        //      {
        //          metricsProviderBuilder
        //              .AddAspNetCoreInstrumentation()
        //              .AddHttpClientInstrumentation()
        //              .AddPrometheusExporter(); // Exporta métricas no formato Prometheus
        //      });

    }
}
