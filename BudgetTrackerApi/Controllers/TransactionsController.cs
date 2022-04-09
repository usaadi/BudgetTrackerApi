namespace BudgetTrackerApi.Controllers;

using Application.Features.Transaction.Commands.CreateTransaction;
using Application.Features.Transaction.Commands.UpdateTransaction;
using Application.Features.Transaction.Commands.DeleteTransaction;
using Application.Features.Transaction.Queries.GetTransactions;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class TransactionsController : ApiControllerBase
{
    [HttpPost("expenses")]
    [Authorize]
    public async Task<ActionResult<TransactionsDto>> GetExpensesTransactions(GetTransactionsQuery query)
    {
        query.TransactionType = TransactionType.Expenses;
        var obj = await Mediator.Send(query);
        return obj;
    }

    [HttpPost("income")]
    [Authorize]
    public async Task<ActionResult<TransactionsDto>> GetIncomeTransactions(GetTransactionsQuery query)
    {
        query.TransactionType = TransactionType.Income;
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<long>> Create(CreateTransactionCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<ActionResult<TransactionDto>> Update(UpdateTransactionCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpDelete("{uniqueId}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid uniqueId)
    {
        var command = new DeleteTransactionCommand()
        {
            UniqueId = uniqueId
        };
        await Mediator.Send(command);
        return NoContent();
    }
}
