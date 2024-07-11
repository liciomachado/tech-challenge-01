﻿using TechChallenge01.Domain.Core;

namespace TechChallenge01.Domain.Interfaces;

public interface IContactRepository : IRepository
{
    Task<Contact?> GetByIdAsync(long id);
    void Save(Contact product);
    void Update(Contact product);
    void Delete(Contact contact);
    Task<List<Contact>> GetAll();
    Task<List<Contact>> GetByDDD(int value);
}
