using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Application.Interfaces
{
    public interface IUpdateContactUseCase
    {
        Task<ContactResponse> Execute(UpdateContactRequest updateContactRequest);
    }
}
