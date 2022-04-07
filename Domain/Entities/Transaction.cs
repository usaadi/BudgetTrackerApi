namespace Domain.Entities;

using Domain.Common;
using Domain.Enums;

public class Transaction : BaseEntity, IUserSpecific
{
    public int TransactionTypeLookupId { get; set; }
    public long CategoryId { get; set; }
    public Guid UserUniqueId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Description { get; set; }

    public TransactionTypeLookup TransactionTypeLookup { get; set; } = null!;
    public Category Category { get; set; } = null!;
}
