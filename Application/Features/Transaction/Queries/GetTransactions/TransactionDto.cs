using Application.Common;
using Domain.Enums;

namespace Application.Features.Transaction.Queries.GetTransactions;

public class TransactionDto : BaseDto
{
    public TransactionType TransactionType { get; set; }
    public string CategoryName { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Description { get; set; }
}
