using Application.Common.Interfaces;
using BudgetTrackerApi.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTrackerApi.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserUniqueId
    {
        get
        {
            var strUuid = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == GeneralConstants.UserIdClaimType)?.Value;
            bool uuidOk = Guid.TryParse(strUuid, out Guid uuid);
            if (uuidOk)
            {
                return uuid;
            }
            else
            {
                return null;
            }
        }
    }
}
