using Microsoft.EntityFrameworkCore;
using TechChallenge01.Domain.Core;
using TechChallenge01.Domain.Entities;

namespace TechChallenge01.Infra.Data.Context;

public class DataContext : DbContext, IUnitOfWork
{
    public DbSet<Contact> Contacts { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>().OwnsOne(p => p.PhoneNumber, nomeBuilder =>
        {
            nomeBuilder.Property(n => n.DDD).HasColumnName("ddd");
            nomeBuilder.Property(n => n.Value).HasColumnName("phone_number");
        });


    }

    public async Task<bool> Commit()
    {
        return await base.SaveChangesAsync() > 0;
    }
}
