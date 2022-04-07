namespace BudgetTrackerApi.Controllers;

using Application.Features.Category.Commands.CreateCategory;
using Application.Features.Category.Commands.DeleteCategory;
using Application.Features.Category.Commands.UpdateCategory;
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

    //[HttpGet("income")]
    //[Authorize]
    //public async Task<ActionResult<CategoriesDto>> GetIncomeCategories()
    //{
    //    return await Mediator.Send(
    //        new GetCategoriesQuery { CategoryType = CategoryType.Income });
    //}

    //[HttpPost]
    //[Authorize]
    //public async Task<ActionResult<long>> Create(CreateCategoryCommand command)
    //{
    //    return await Mediator.Send(command);
    //}

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
