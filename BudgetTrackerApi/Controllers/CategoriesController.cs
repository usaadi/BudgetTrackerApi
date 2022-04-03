namespace BudgetTrackerApi.Controllers;

using Application.Features.Category.Queries.GetCategories;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class CategoriesController : ApiControllerBase
{
    [HttpGet("expenses")]
    public async Task<ActionResult<CategoriesDto>> GetExpensesCategories()
    {
        return await Mediator.Send(
            new GetCategoriesQuery { CategoryType = CategoryType.Expenses, UserUniqueId = Guid.Empty });
    }

    [HttpGet("income")]
    public async Task<ActionResult<CategoriesDto>> GetIncomeCategories()
    {
        return await Mediator.Send(
            new GetCategoriesQuery { CategoryType = CategoryType.Income, UserUniqueId = Guid.Empty });
    }
}
