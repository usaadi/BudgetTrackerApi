namespace BudgetTrackerApi.Controllers;

using Application.Features.Transaction.Commands.CreateTransaction;
using Application.Features.Transaction.Queries.GetTransactions;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class TransactionsController : ApiControllerBase
{
    [HttpGet("expenses")]
    [Authorize]
    public async Task<ActionResult<TransactionsDto>> GetExpensesTransactions()
    {
        return await Mediator.Send(
            new GetTransactionsQuery { TransactionType = TransactionType.Expenses });
    }

    [HttpGet("income")]
    [Authorize]
    public async Task<ActionResult<TransactionsDto>> GetIncomeTransactions()
    {
        return await Mediator.Send(
            new GetTransactionsQuery { TransactionType = TransactionType.Income });
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<long>> Create(CreateTransactionCommand command)
    {
        return await Mediator.Send(command);
    }

    //[HttpPatch("{uniqueId}")]
    //[Authorize]
    //public async Task<ActionResult<CategoryDto>> Update(UpdateCategoryCommand command, Guid uniqueId)
    //{
    //    command.uniqueId = uniqueId;
    //    return await Mediator.Send(command);
    //}

    //[HttpDelete("{uniqueId}")]
    //[Authorize]
    //public async Task<ActionResult> Delete(Guid uniqueId)
    //{
    //    var command = new DeleteCategoryCommand();
    //    command.UniqueId = uniqueId;
    //    await Mediator.Send(command);
    //    return NoContent();
    //}
}
