using Microsoft.EntityFrameworkCore;
using TechChallenge01.Domain;
using TechChallenge01.Domain.Core;
using TechChallenge01.Domain.Interfaces;

namespace TechChallenge01.Infra.Data;

public class ContactRepository(DataContext dataContext) : IContactRepository
{
    public IUnitOfWork UnitOfWork => dataContext;

    public void Dispose()
    {
        dataContext.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task<Contact?> GetByIdAsync(long id)
    {
        return await dataContext.Contacts.FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Save(Contact contact)
    {
        dataContext.Contacts.Add(contact);
    }

    public void Update(Contact contact)
    {
        dataContext.Contacts.Update(contact);
    }

    public void Delete(Contact contact)
    {
        dataContext.Contacts.Remove(contact);
    }

    public async Task<List<Contact>> GetAll()
    {
        return await dataContext.Contacts.ToListAsync();
    }
}
