using Core.Domain;
using Microsoft.AspNetCore.Identity;
using WebApi.Models;
using WebApi.Models.Auth;

namespace WebApi.Auth;

public interface IUserRepositoryAuth
{
    public Task<UserAuthDto?> GetUserForToken(UserAuthDto user);
    public Task<UserWindAuthDto?> GetUserWindAuthForToken(UserAuthDto user);
    public Task<UserWindAuthDto?> GetUserWindAuthForRefreshToken(string userId);
    public Task<UserAuthDto?> UpdateUserPassword(UserAuthDto user, string passwordNew);
    public Task<UserAuthDto?> CreateUserPassword(UserAuthDto user, UserWind? userEf=null);
    public Task<UserWindWithToken> CreateUsersWithToken(string userName, string password, string email, string role);
    public Task<UserAuthDto?> DeleteUserPassword(UserAuthDto user);
    public Task InitUsers(List<UserAuthDto> UserLists);
    public Task<IEnumerable<IdentityRole<Guid>>?> GetRolesAsync();
    public Task<UserRolesResponse?> GetUserRolesAsync(Guid id);
    public Task<UserRolesResponse?> CreateUserRolesAsync(Guid id, string role);
    public Task<UserRolesResponse?> DeleteUserRolesAsync(Guid id, string role);
 
}