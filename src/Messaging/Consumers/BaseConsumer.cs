using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Messaging.Interface;
namespace Messaging.Consumers
{
    public class MessageConsumer<T> : IConsumer<T> where T : class
    {
        private readonly IMessageConsumer<T> _messageConsumer;

        public MessageConsumer(IMessageConsumer<T> messageConsumer)
        {
            _messageConsumer = messageConsumer;
        }

        public async Task Consume(ConsumeContext<T> context)
        {
            await _messageConsumer.ConsumeMessage(context.Message);
        }
    }
}
