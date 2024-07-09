using Microsoft.EntityFrameworkCore;
using TechChallenge01.Domain;
using TechChallenge01.Domain.Core;

namespace TechChallenge01.Infra.Data;

public class DataContext : DbContext, IUnitOfWork
{
    public DbSet<Contact> Contacts { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public async Task<bool> Commit()
    {
        return await base.SaveChangesAsync() > 0;
    }
}
