using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using TechChallenge01.Application.Consumers;
using TechChallenge01.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Cadastro de Contatos", Version = "v1.0" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
        "JWT Authorization Header - utilizado com Bearer Authentication.\r\n\r\n" +
        "Digite 'Bearer' [espa�o] e ent�o seu token no campo abaixo.\r\n\r\n" +
        "Exemplo (infomrar sem as aspas): 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddInjections(builder.Configuration);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretJWT"));

//// Configura��o do MassTransit
//builder.Services.AddMassTransit(x =>
//{
//    // Registra o consumidor espec�fico
//    x.AddConsumer<InsertContactConsumer>();


//    // Configura o RabbitMQ
//    x.UsingRabbitMq((context, cfg) =>
//    {
//        var rabbitMqHost = configuration["RabbitMQ:RABBITMQ_HOST"];
//        var rabbitMqPort = int.Parse(configuration["RabbitMQ:RABBITMQ_PORT"]); // Certifique-se de que � int
//        var rabbitMqUser = configuration["RabbitMQ:RABBITMQ_USER"];
//        var rabbitMqPassword = configuration["RabbitMQ:RABBITMQ_PASSWORD"];

//        // Configura��o do Host do RabbitMQ
//        cfg.Host(rabbitMqHost, (ushort)rabbitMqPort, "/", h => // Certifique-se de que o porto � ushort
//        {
//            h.Username(rabbitMqUser);
//            h.Password(rabbitMqPassword);
//        });

//        // Configura��o autom�tica dos endpoints
//        cfg.ConfigureEndpoints(context);
//    });
//});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(j =>
    {
        j.RequireHttpsMetadata = false;
        j.SaveToken = true;
        j.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();
app.MapPrometheusScrapingEndpoint();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program;