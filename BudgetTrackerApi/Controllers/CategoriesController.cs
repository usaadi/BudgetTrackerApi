namespace BudgetTrackerApi.Controllers;

using Application.Features.Category.Commands.CreateCategory;
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
            new GetCategoriesQuery { CategoryType = CategoryType.Expenses });
    }

    [HttpGet("income")]
    [Authorize]
    public async Task<ActionResult<CategoriesDto>> GetIncomeCategories()
    {
        return await Mediator.Send(
            new GetCategoriesQuery { CategoryType = CategoryType.Income });
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<long>> Create(CreateCategoryCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPatch("{uniqueId}")]
    [Authorize]
    public async Task<ActionResult<CategoryDto>> Update(UpdateCategoryCommand command, Guid uniqueId)
    {
        command.uniqueId = uniqueId;
        return await Mediator.Send(command);
    }
}
