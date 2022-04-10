namespace BudgetTrackerApi.Controllers;

using Application.Features.Category.Commands.CreateCategory;
using Application.Features.Category.Commands.DeleteCategory;
using Application.Features.Category.Commands.UpdateCategory;
using Application.Features.Category.Queries.GetCategories;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class CategoriesController : ApiControllerBase
{
    [HttpGet("expenses")]
    [Authorize]
    public async Task<ActionResult<CategoriesDto>> GetExpensesCategories()
    {
        return await Mediator.Send(
            new GetCategoriesQuery { TransactionType = TransactionType.Expenses });
    }

    [HttpGet("income")]
    [Authorize]
    public async Task<ActionResult<CategoriesDto>> GetIncomeCategories()
    {
        return await Mediator.Send(
            new GetCategoriesQuery { TransactionType = TransactionType.Income });
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<long>> Create(CreateCategoryCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<ActionResult<CategoryDto>> Update(UpdateCategoryCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpDelete("{uniqueId}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid uniqueId)
    {
        var command = new DeleteCategoryCommand
        {
            UniqueId = uniqueId
        };
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("with-related-data/{uniqueId}")]
    [Authorize]
    public async Task<ActionResult> DeleteWithRelatedData(Guid uniqueId)
    {
        var command = new DeleteCategoryCommand
        {
            UniqueId = uniqueId,
            AllowDeleteRelatedData = true
        };
        await Mediator.Send(command);
        return NoContent();
    }
}
