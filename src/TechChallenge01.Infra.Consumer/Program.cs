using TechChallenge01.Infra.Consumer;
using TechChallenge01.Infra.Consumer.IoC;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddInjections(builder.Configuration);

var host = builder.Build();
host.Run();
