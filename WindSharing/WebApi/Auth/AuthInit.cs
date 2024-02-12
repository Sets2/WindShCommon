using WebApi.Models.Auth;

namespace WebApi.Auth;

public class AuthInit: IAuthInit
{
    private IUserRepositoryAuth _userRepositoryAuth;

    private readonly List<UserAuthDto> UserLists = new List<UserAuthDto>
    {
        new UserAuthDto("Ivanov", "Aa_123", "admin"),
        new UserAuthDto("Petrov", "Uu_123", "user")
    };

    public AuthInit(IUserRepositoryAuth userRepositoryAuth)
    {
        _userRepositoryAuth= userRepositoryAuth;
    }
    public async Task UserAuthInit()
    {
        await _userRepositoryAuth.InitUsers(UserLists);
    }
}