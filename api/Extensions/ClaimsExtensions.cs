using System.Security.Claims;

namespace api.Extensions;

public static class ClaimsExtensions
{
    public static string GetUsername(this ClaimsPrincipal user)
    {
        var claim = user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"));

        if (claim == null)
        {
            throw new UnauthorizedAccessException("Username claim not found in token");
        }

        return claim.Value;
    }
}