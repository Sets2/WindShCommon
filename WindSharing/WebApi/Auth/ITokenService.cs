using WebApi.Models.Auth;

namespace WebApi.Auth;

public interface ITokenService
{
    string BuildToken(UserAuthDto user);
}