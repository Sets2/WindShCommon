using Microsoft.AspNetCore.Authorization;

namespace WebApi.Auth;

public class AdminOrOwnerRequirement: IAuthorizationRequirement
{
    public const string Name = "AdminOrOwner";
}

public class OwnerRequirement : IAuthorizationRequirement
{
    public const string Name = "Owner";
}