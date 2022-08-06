using Application.Features.Profile.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTrackerApi.Controllers;

[ApiController]
public class ProfileController : ApiControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ProfileDto>> GetExpensesTransactions()
    {
        var query = new GetProfileQuery();
        var obj = await Mediator.Send(query);
        return obj;
    }
}
