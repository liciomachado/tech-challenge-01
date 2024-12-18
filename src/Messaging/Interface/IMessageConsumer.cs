using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Interface
{
    public interface IMessageConsumer<in T>
    {
        Task ConsumeMessage(T message);
    }
}
