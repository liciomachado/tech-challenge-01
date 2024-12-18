using MassTransit;
using Messaging.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Extensions
{
    public static class MessagingConfiguration
    {
        public static void ConfigureMessaging(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<IBusRegistrationConfigurator>? configureConsumers = null,
            Action<IRabbitMqBusFactoryConfigurator, IBusRegistrationContext>? configureReceiveEndpoints = null)
        {
            services.AddMassTransit(x =>
            {
                // Configura consumidores fornecidos pelo chamador
                configureConsumers?.Invoke(x);

                // Configura o RabbitMQ
                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitMqHost = configuration["RabbitMQ:RABBITMQ_HOST"] ?? "localhost";
                    var rabbitMqPort = int.TryParse(configuration["RabbitMQ:RABBITMQ_PORT"], out var port) ? port : 5672; // Padrão 5672
                    var rabbitMqUser = configuration["RabbitMQ:RABBITMQ_USER"] ?? "guest";
                    var rabbitMqPassword = configuration["RabbitMQ:RABBITMQ_PASSWORD"] ?? "guest";

                    
                    cfg.Host(rabbitMqHost, h =>
                    {
                        h.Username(rabbitMqUser);
                        h.Password(rabbitMqPassword);
                    });
                    // Configura os endpoints, permitindo sobrescrita pelo chamador
                    if (configureReceiveEndpoints != null)
                    {
                        configureReceiveEndpoints(cfg, context);
                    }
                    else
                    {
                        // Configuração padrão dos endpoints
                        cfg.ConfigureEndpoints(context);
                    }
                });
            });
        }
    }

}
