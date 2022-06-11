namespace Application.Features.Transaction.Queries.GetTransactions;

public class TransactionsDto
{
    public TransactionsDto()
    {
        Items = new List<TransactionDto>();
    }

    public IList<TransactionDto> Items { get; set; }
    public int TotalCount { get; set; }
}
