using Core.Domain;

namespace WebApi.Models.Auth;

public class UserWindAuthDto
{
    public UserWind UserWind { get; set; }
    public UserAuthDto UserAuth { get; set; }

    public UserWindAuthDto(UserWind user, UserAuthDto auth)
    {
        UserWind=user;
        UserAuth=auth;
    }
}