using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallenge01.Application.Events;

namespace TechChallenge01.Application.Interfaces
{
    public  interface IContactPublisher
    {
        Task PublishContactAsync(InsertContactEvent message);
    }
}
