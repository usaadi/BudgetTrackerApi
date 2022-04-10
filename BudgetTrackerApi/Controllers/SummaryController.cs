namespace BudgetTrackerApi.Controllers;

using Application.Features.Summary.Queries.GetSummary;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class SummaryController : ApiControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<SummaryDto>> GetExpensesTransactions(GetSummaryQuery query)
    {
        var obj = await Mediator.Send(query);
        return obj;
    }
}
