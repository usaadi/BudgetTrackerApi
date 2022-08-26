namespace Application.Common.Interfaces;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public interface IApplicationDbContext
{
    DbSet<Category> Category { get; }
    DbSet<Transaction> Transaction { get; }
    DbSet<TransactionTypeLookup> TransactionTypeLookup { get; }
    DbSet<UserSetting> UserSetting { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task MigrateAsync();
}
