using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;

namespace WebApi.Auth;

public class CustomHandler: IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        var pendingRequirements = context.PendingRequirements.ToList();

        foreach (var requirement in pendingRequirements)
        {
            if (requirement is AdminOrOwnerRequirement && context.User.IsInRole("admin"))
            {
                    context.Succeed(requirement);
                    break;
            }
            if(requirement is OwnerRequirement || requirement is AdminOrOwnerRequirement)
            {
                if (IsOwnerId(context.User, context.Resource) || IsOwnerName(context.User, context.Resource))
                {
                    context.Succeed(requirement);
                }
            }
        }
        return Task.CompletedTask;
    }

    private static bool IsOwnerId(ClaimsPrincipal user, object? resource)
    {
        var stringId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (stringId == null)
        {
            return false;
        }

        string? requestId = null;

        //var resClameIdGuid = Guid.TryParse(stringId, out var clameIdGuid);
        //if (!resClameIdGuid)
        //{
        //    return false;
        //}

        
        StringValues queryId;
        if (resource is HttpContext httpContext)
        {
            // проверяем параметры запроса
            var tryId = httpContext.Request.Query.TryGetValue("id", out queryId);
            if (!tryId)
            {
                // проверяем путь
                var lenPath = httpContext.Request.Path.Value.Length;
                if (lenPath > 36)
                {
                    requestId = httpContext.Request.Path.Value.Substring(lenPath - 36);
                    tryId = Guid.TryParse(stringId, out var guidResult);
                    if(! tryId) { return false; }
                }
                else return false;
            }
            else requestId = queryId;
        }
        else return false;

        return stringId== requestId;
    }

    private static bool IsOwnerName(ClaimsPrincipal user, object? resource)
    {
        var stringName = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        if (stringName == null)
        {
            return false;
        }

        StringValues queryName;
        if (resource is HttpContext httpContext)
        {
            var tryName = httpContext.Request.Query.TryGetValue("username", out queryName);
            if (!tryName)
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        return Guid.TryParse(queryName, out var queryIdGuid) &&
               queryIdGuid.Equals(stringName);
    }
}