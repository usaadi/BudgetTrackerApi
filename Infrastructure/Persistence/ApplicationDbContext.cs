using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Category> Category => Set<Category>();
    public DbSet<Transaction> Transaction => Set<Transaction>();
    public DbSet<TransactionTypeLookup> TransactionTypeLookup => Set<TransactionTypeLookup>();

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Category>().HasQueryFilter(p => !p.IsDeleted && p.UserUniqueId == currentUserUniqueId);
        modelBuilder.Entity<Category>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Transaction>().HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    private void PrepareSaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    //entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.Created = DateTime.UtcNow;
                    entry.Entity.UniqueId = Guid.NewGuid();
                    break;

                case EntityState.Modified:
                    //entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    entry.Entity.LastModified = DateTime.UtcNow;
                    break;
            }
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        PrepareSaveChanges();

        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    public override int SaveChanges()
    {
        PrepareSaveChanges();

        return base.SaveChanges();
    }

    public async Task MigrateAsync()
    {
        await Database.MigrateAsync();
    }
}
