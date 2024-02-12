using Core.Domain;

namespace WebApi.Models.Auth;

public class UserWindWithToken
{
    public UserWindResponse? UserWind { get; set; }
    public string? Token { get; set; }
    public string? Error { get; set; }

    public UserWindWithToken()
    {
    }

    public UserWindWithToken(string userName, string email)
    {
        UserWind = new UserWindResponse
        {
            UserName = userName,
            Email = email
        };
    }
    public UserWindWithToken(string error)
    {
        Error = Error;
    }
}