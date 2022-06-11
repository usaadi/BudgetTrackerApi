namespace Application.Features.TransactionsSummary.Queries.GetTransactionsSummary
{
    public class TransactionsSummaryItemDto
    {
        public string CategoryName { get; set; } = null!;
        public decimal Sum { get; set; }
    }
}
