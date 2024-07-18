using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;
using WebApi.Domain.ValueObjects;
using WebApi.Infrastructure.Context.Builders;

namespace WebApi.Infrastructure.Context;

public class TechChallenge1DbContext : DbContext
{
    public TechChallenge1DbContext()
    {
    }

    public TechChallenge1DbContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<Contact> Contacts { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ContactConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }

    public async Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        await Database.MigrateAsync(cancellationToken);
    }
}