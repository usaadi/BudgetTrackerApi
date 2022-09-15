using Application.Common.Interfaces;
using BudgetTrackerApi.Constants;
using System.Security.Claims;

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

    public string? Email
    {
        get
        {
            var email = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == GeneralConstants.EmailClaimType)?.Value;
            return email;
        }
    }

    public string? FullName
    {
        get
        {
            var fullName = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == GeneralConstants.NameClaimType)?.Value;
            return fullName;
        }
    }
}
