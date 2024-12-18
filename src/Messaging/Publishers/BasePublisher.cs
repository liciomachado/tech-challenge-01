using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Messaging.Interface;
namespace Messaging.Publishers;

public abstract class BasePublisher<T> where T : class
{
    private readonly IBus _bus;

    // Construtor que recebe o IBus para enviar as mensagens
    public BasePublisher(IBus bus)
    {
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    }

    // Método para publicar uma mensagem
    public async Task PublishAsync(T message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message), "Message cannot be null");
        }

        // Envia a mensagem para o RabbitMQ
        await _bus.Publish(message);
    }
}