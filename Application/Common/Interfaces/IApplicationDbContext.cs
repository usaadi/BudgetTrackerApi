namespace Application.Common.Interfaces;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;

public interface IApplicationDbContext
{
    DbSet<Category> Category { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
