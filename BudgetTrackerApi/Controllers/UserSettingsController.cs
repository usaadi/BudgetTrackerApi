using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Features.UserSetting.Queries.GetUserSettings;
using Application.Features.UserSetting.Commands.SetUserSettings;
using Application.Features.UserSetting.Commands.PatchUserSettings;

namespace BudgetTrackerApi.Controllers;

[ApiController]
public class UserSettingsController : ApiControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<UserSettingsDto>> GetUserSettings()
    {
        return await Mediator.Send(new GetUserSettingsQuery());
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<UserSettingsDto>> SetUserSettings(SetUserSettingsCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<ActionResult<UserSettingsDto>> PatchUserSettings(PatchUserSettingsCommand command)
    {
        return await Mediator.Send(command);
    }
}
