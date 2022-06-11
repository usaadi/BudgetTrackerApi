namespace Domain.Entities;

using Domain.Common;
using Domain.Enums;

public class Category : BaseEntity, IUserSpecific
{
    public int TransactionTypeLookupId { get; set; }
    public Guid UserUniqueId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public TransactionTypeLookup TransactionTypeLookup { get; set; } = null!;
}
