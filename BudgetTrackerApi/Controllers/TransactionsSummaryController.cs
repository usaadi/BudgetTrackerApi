namespace BudgetTrackerApi.Controllers;

using Application.Features.TransactionsSummary.Queries.GetTransactionsSummary;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class TransactionsSummaryController : ApiControllerBase
{
    [HttpPost("expenses")]
    [Authorize]
    public async Task<ActionResult<TransactionsSummaryDto>> GetExpensesTransactions(GetTransactionsSummaryQuery query)
    {
        query.TransactionType = TransactionType.Expenses;
        var obj = await Mediator.Send(query);
        return obj;
    }

    [HttpPost("income")]
    [Authorize]
    public async Task<ActionResult<TransactionsSummaryDto>> GetIncomeTransactions(GetTransactionsSummaryQuery query)
    {
        query.TransactionType = TransactionType.Income;
        return await Mediator.Send(query);
    }
}
