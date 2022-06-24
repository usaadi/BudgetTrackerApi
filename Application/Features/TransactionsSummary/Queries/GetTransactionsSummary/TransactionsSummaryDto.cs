namespace Application.Features.TransactionsSummary.Queries.GetTransactionsSummary
{
    public class TransactionsSummaryDto
    {
        public TransactionsSummaryDto()
        {
            Items = new List<TransactionsSummaryItemDto>();
        }

        public List<TransactionsSummaryItemDto> Items { get; set; }
        public int TotalCount { get; set; }
        public int NextPageNumber { get; set; }
        public bool HasMore { get; set; }
    }
}
