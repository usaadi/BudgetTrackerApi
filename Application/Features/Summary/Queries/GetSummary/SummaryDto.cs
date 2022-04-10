namespace Application.Features.Summary.Queries.GetSummary
{
    public class SummaryDto
    {
        public decimal ExpensesSum { get; set; }
        public decimal IncomeSum { get; set; }
        public decimal Balance { get; set; }
    }
}
