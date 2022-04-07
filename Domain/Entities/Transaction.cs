namespace Domain.Entities;

using Domain.Common;
using Domain.Enums;

public class Transaction : BaseEntity, IUserSpecific
{
    public long CategoryId { get; set; }
    public Guid UserUniqueId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Description { get; set; }

    public Category Category { get; set; } = null!;
}
