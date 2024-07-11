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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>().OwnsOne(p => p.DDD, nomeBuilder =>
        {
            nomeBuilder.Property(n => n.Value).HasColumnName("ddd");
        });
    }

    public async Task<bool> Commit()
    {
        return await base.SaveChangesAsync() > 0;
    }
}
