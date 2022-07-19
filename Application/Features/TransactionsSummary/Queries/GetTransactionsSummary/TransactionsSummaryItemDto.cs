namespace Application.Features.TransactionsSummary.Queries.GetTransactionsSummary
{
    public class TransactionsSummaryItemDto
    {
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Sum { get; set; }
    }
}
