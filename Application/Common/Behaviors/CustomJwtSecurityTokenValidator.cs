using Application.Common.Constants;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Application.Common.Behaviors;

public class CustomJwtSecurityTokenValidator : ISecurityTokenValidator
{
    private string DemoToken { get; }

    public CustomJwtSecurityTokenValidator(string demoToken)
    {
        DemoToken = demoToken;
    }

    public bool CanValidateToken => true;
    public int MaximumTokenSizeInBytes { get; set; } = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;

    public bool CanReadToken(string securityToken)
    {
        return true;
    }

    public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
    {
        validatedToken = new JsonWebToken(securityToken);

        if (securityToken == DemoToken)
        {
            var claim = new Claim(GeneralConstants.UserIdClaimType, GeneralConstants.DemoAccountUUID);
            var identity = new ClaimsIdentity(new Claim[] { claim }, "test");
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }

        return new();
    }
}
