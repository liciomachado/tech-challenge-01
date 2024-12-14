using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("ocelot.json")
    .Build();
// Adiciona Ocelot com Polly (Circuit Breaker)
builder.Services.AddOcelot().AddPolly();

// Configuração OpenTelemetry
var serviceName = "APIGateway";
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder => tracerProviderBuilder
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        )
    .WithMetrics(metricsProviderBuilder => metricsProviderBuilder
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddRuntimeInstrumentation()
        .AddProcessInstrumentation()
        .AddPrometheusExporter());

var app = builder.Build();

app.UseOcelot().Wait();

app.Run();
