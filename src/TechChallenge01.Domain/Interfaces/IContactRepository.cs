using TechChallenge01.Domain.Core;
using TechChallenge01.Domain.Entities;

namespace TechChallenge01.Domain.Interfaces;

public interface IContactRepository : IRepository
{
    Task<Contact?> GetByIdAsync(long id);
    void Save(Contact contact);
    void Update(Contact contact);
    void Delete(Contact contact);
    Task<List<Contact>> GetAll();
    Task<List<Contact>> GetByDDD(string? value);
}
