using Microsoft.EntityFrameworkCore;
using TechChallenge01.Domain.Core;
using TechChallenge01.Domain.Entities;
using TechChallenge01.Domain.Interfaces;
using TechChallenge01.Infra.Data.Context;

namespace TechChallenge01.Infra.Data.Repositories;

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
        return await dataContext.Contacts.OrderBy(x => x.Id).ToListAsync();
    }

    public async Task<List<Contact>> GetByDDD(string? value)
    {
        return await dataContext.Contacts.Where(x => x.PhoneNumber.DDD == value).ToListAsync();
    }
}
