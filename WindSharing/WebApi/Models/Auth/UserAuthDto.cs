using Microsoft.AspNetCore.Mvc;

namespace WebApi.Models.Auth;

public class UserAuthDto
{
    public Guid? Id;
    public string? UserName;
    public string Password = "";
    public string Role = "";
    public string SecurityStamp = "";
    public List<string>? Roles = null;

    public UserAuthDto() : base()
    {
    }

    public UserAuthDto(Guid? id, string? userName, string securityStamp)
    {
        Id = id;
        UserName = userName;
        SecurityStamp = securityStamp;
    }

    public UserAuthDto(string? userName, string? password, string? role)
    {
        UserName = userName;
        Password = password;
        Role = role;
    }

    public UserAuthDto(string? userName, string? password)
    {
        UserName = userName;
        Password = password;
    }
    public UserAuthDto(string? userName)
    {
        UserName = userName;
    }

    public UserAuthDto(Guid userId)
    {
        Id = userId;
    }

}