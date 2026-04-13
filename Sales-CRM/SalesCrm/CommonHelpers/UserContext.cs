using System.Security.Claims;

public static class UserContext
{
    public static int GetUserId(ClaimsPrincipal user)
    {
        return Convert.ToInt32(user.FindFirst("id")?.Value);
    }

    public static int GetCompanyId(ClaimsPrincipal user)
    {
        return Convert.ToInt32(user.FindFirst("company_id")?.Value);
    }
}