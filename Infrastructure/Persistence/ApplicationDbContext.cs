using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Common;
using System.Reflection;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Category> Category => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Category>().HasQueryFilter(p => !p.IsDeleted && p.UserUniqueId == currentUserUniqueId);
        modelBuilder.Entity<Category>().HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
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

        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
}
