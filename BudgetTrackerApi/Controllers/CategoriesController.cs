using Microsoft.AspNetCore.Mvc;

namespace BudgetTrackerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static readonly string[] ExpensesCategories = new[]
        {
        "Groceries", "Car Gas", "Heating", "Electricity", "Water", "Clothes", "Eating Out", "Take Out"
        };

        private static readonly string[] IncomeCategories = new[]
        {
        "Salary", "Rent"
        };

        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ILogger<CategoriesController> logger)
        {
            _logger = logger;
        }

        [HttpGet("expenses")]
        public async Task<ActionResult<IEnumerable<string>>> GetExpensesCategories()
        {
            return await Task.FromResult(ExpensesCategories);
        }

        [HttpGet("income")]
        public async Task<ActionResult<IEnumerable<string>>> GetIncomeCategories()
        {
            return await Task.FromResult(IncomeCategories);
        }
    }
}