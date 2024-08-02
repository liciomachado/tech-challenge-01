using TechChallenge01.Application.Interfaces;
using TechChallenge01.Application.ViewModels;
using TechChallenge01.Domain.Interfaces;
using TechChallenge01.Domain.Validations;
using TechChallenge01.Domain.ValueObjects;

namespace TechChallenge01.Application.UseCases
{
    public class UpdateContactUseCase(IContactRepository contactRepository) : IUpdateContactUseCase
    {
        public async Task<ContactResponse> Execute(UpdateContactRequest updateContactRequest)
        {
            var contact = await contactRepository.GetByIdAsync(updateContactRequest.Id);
            if (contact == null) throw new ApplicationException("Não foi possível localizar o cadastro do contato informado.");

            if (!ContactValidator.IsValidName(updateContactRequest.Name))
                throw new ArgumentException("O nome é obrigatório.");

            if (!ContactValidator.IsValidEmail(updateContactRequest.Email))
                throw new ArgumentException("Formato de e-mail inválido.");

            var phoneNumber = new PhoneNumber(updateContactRequest.PhoneNumber);
            contact.Update(updateContactRequest.Name, phoneNumber, updateContactRequest.Email);
            contactRepository.Update(contact);
            await contactRepository.UnitOfWork.Commit();

            return new ContactResponse(contact.Id, contact.Name, contact.PhoneNumber.Value, contact.Email, contact.PhoneNumber.DDD);
        }
    }
}
