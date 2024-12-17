using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Reflection;
using TechChallenge01.Infra.Token.IoC;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Autenticacao Cadastro de Contatos", Version = "v1.0" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddInjections(builder.Configuration);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var app = builder.Build();
app.MapPrometheusScrapingEndpoint();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});
app.UseHttpsRedirection();
app.MapControllers();
// Mapear os Endpoints de Health Checks
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new { e.Key, Status = e.Value.Status.ToString() })
        });
        await context.Response.WriteAsync(result);
    }
});
app.Run();

public partial class Program;