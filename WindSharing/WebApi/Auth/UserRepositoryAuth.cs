using Core.Abstractions.Repositories;
using Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApi.Models;
using WebApi.Models.Auth;

namespace WebApi.Auth;

public class UserRepositoryAuth : IUserRepositoryAuth
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<UserWind> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IRepository<UserWind> _userWindRepository;
    private readonly SignInManager<UserWind> _signInManager;
    private readonly ILogger<UserRepositoryAuth> _logger;
    private readonly ITokenService _tokenService;

    public UserRepositoryAuth(UserManager<UserWind> userManager,
        IHttpContextAccessor httpContextAccessor,
        RoleManager<IdentityRole<Guid>> roleManager, IRepository<UserWind> userWindRepository,
        SignInManager<UserWind> signInManager, ILogger<UserRepositoryAuth> logger, ITokenService tokenService)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _roleManager = roleManager;
        _userWindRepository = userWindRepository;
        _signInManager = signInManager;
        _logger = logger;
        _tokenService = tokenService;
    }
    public async Task<UserAuthDto?> GetUserForToken(UserAuthDto user)
    {
        UserWind? userEf;
        if (user.Id == null) userEf = await _userManager.FindByNameAsync(user.UserName);
        else userEf = await _userManager.FindByIdAsync(user.Id.ToString());

        if (userEf != null)
        {
            var result = await _signInManager.PasswordSignInAsync(userEf, user.Password, true, false);
            if (result.Succeeded)
            {
                UserAuthDto userDto = new(userEf.Id, userEf.UserName, userEf.SecurityStamp);
                userDto.Roles = (List<string>)await _userManager.GetRolesAsync(userEf);
                return userDto;
            }
        }
        return null;
    }

    public async Task<UserWindAuthDto?> GetUserWindAuthForToken(UserAuthDto user)
    {
        UserWind? userEf;
        if (user.Id == null) userEf = await _userManager.FindByNameAsync(user.UserName);
        else userEf = await _userManager.FindByIdAsync(user.Id.ToString());

        if (userEf != null)
        {
            var result = await _signInManager.PasswordSignInAsync(userEf, user.Password, true, false);
            if (result.Succeeded)
            {
                UserAuthDto userDto = new(userEf.Id, userEf.UserName, userEf.SecurityStamp);
                userDto.Roles = (List<string>)await _userManager.GetRolesAsync(userEf);
                return new UserWindAuthDto(userEf, userDto);
            }
        }
        return null;
    }

    public async Task<UserWindAuthDto?> GetUserWindAuthForRefreshToken(string userId)
    {
       
        UserWind? userEf;
        userEf = await _userManager.FindByIdAsync(userId);

        if (userEf != null)
        {
            UserAuthDto userDto = new(userEf.Id, userEf.UserName, userEf.SecurityStamp);
            userDto.Roles = (List<string>)await _userManager.GetRolesAsync(userEf);
            return new UserWindAuthDto(userEf, userDto);
        }

        return null;
    }


    public async Task<UserAuthDto?> CreateUserPassword(UserAuthDto user, UserWind? userEf = null)
    {
        if (userEf == null)
        {
            if (user.Id == null) userEf = await _userManager.FindByNameAsync(user.UserName);
            else userEf = await _userManager.FindByIdAsync(user.Id.ToString());
        };
        if (userEf != null)
        {
            var result = await _userManager.AddPasswordAsync(userEf, user.Password);
            if (result.Succeeded)
            {
                UserAuthDto userDto = new(userEf.Id, userEf.UserName, user.SecurityStamp);
                //                userDto.SecurityStamp = userEf.SecurityStamp;
                userDto.Roles = (List<string>)await _userManager.GetRolesAsync(userEf);
                return userDto;
            }
            else return null;
        }
        else return null;
    }

    public async Task<UserAuthDto?> UpdateUserPassword(UserAuthDto user, string passwordNew)
    {
        UserWind? userEf;
        if (user.Id == null) userEf = await _userManager.FindByNameAsync(user.UserName);
        else userEf = await _userManager.FindByIdAsync(user.Id.ToString());
        if (userEf != null)
        {
            var result = await _userManager.ChangePasswordAsync(userEf, user.Password, passwordNew);
            if (result.Succeeded)
            {
                UserAuthDto userDto = new(userEf.Id, userEf.UserName, user.SecurityStamp);
                //                userDto.SecurityStamp = userEf.SecurityStamp;
                userDto.Roles = (List<string>)await _userManager.GetRolesAsync(userEf);
                return userDto;
            }
            else return null;
        }
        else return null;
    }

    public async Task<UserAuthDto?> DeleteUserPassword(UserAuthDto user)
    {
        UserWind? userEf;
        if (user.Id == null) userEf = await _userManager.FindByNameAsync(user.UserName);
        else userEf = await _userManager.FindByIdAsync(user.Id.ToString());
        if (userEf != null)
        {
            var result = await _userManager.RemovePasswordAsync(userEf);
            if (result.Succeeded)
            {
                UserAuthDto userDto = new(userEf.Id, userEf.UserName, user.SecurityStamp);
                //                userDto.SecurityStamp = userEf.SecurityStamp;
                userDto.Roles = (List<string>)await _userManager.GetRolesAsync(userEf);
                return userDto;
            }
            else return null;
        }
        else return null;
    }

    public async Task InitUsers(List<UserAuthDto> userLists)
    {
        foreach (var userDto in userLists)
        {
            try
            {
                if (await _roleManager.FindByNameAsync(userDto.Role) == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(userDto.Role));
                }

                UserWind? userFind = await _userWindRepository.GetFirstWhere(x => x.UserName == userDto.UserName);
                if (userFind == null)
                {
                    UserWind user = new UserWind { UserName = userDto.UserName, IsActive = true };

                    var result = await _userManager.CreateAsync(user, userDto.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, userDto.Role);
                    }
                }
                else
                {
                    await _userManager.AddPasswordAsync(userFind, userDto.Password);
                    await _userManager.UpdateNormalizedUserNameAsync(userFind);
                    await _userManager.UpdateNormalizedEmailAsync(userFind);
                    await _userManager.AddToRoleAsync(userFind, userDto.Role);
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation(
                    $"Ошибка при инициализации пользователя {userDto.UserName}: {e.Message})");
            }
        }
    }
    public async Task<UserWindWithToken> CreateUsersWithToken(string userName, string password,
        string email, string role)
    {
        UserWind user = new UserWind { UserName = userName, Email = email, IsActive = true };
        var response = new UserWindWithToken(userName, email);

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            /*foreach (var error in result.Errors)
                response.Errors.Add(error.Description);
            return response;*/
            response.Error = string.Join(", ", result.Errors.Select(x => x.Description));
            return response;
        }

        response.UserWind.Id = user.Id;

        var resRole = await _userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded)
        {
            //foreach (var error in resRole.Errors)
            //    response.Errors.Add(error.Description);
            response.Error = string.Join(", ", result.Errors.Select(x => x.Description));
            return response;
        }

        UserAuthDto userDto = new(user.Id, user.UserName, user.SecurityStamp);
        response.Token = _tokenService.BuildToken(userDto);

        return response;
    }

    public async Task<IEnumerable<IdentityRole<Guid>>?> GetRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return roles;
    }

    public async Task<UserRolesResponse?> GetUserRolesAsync(Guid id)
    {
        var userEf = await _userManager.FindByIdAsync(id.ToString());
        if (userEf == null) return null;
        var roles = await _userManager.GetRolesAsync(userEf);
        return new UserRolesResponse(userEf, roles);
    }

    public async Task<UserRolesResponse?> CreateUserRolesAsync(Guid id, string role)
    {
        var userEf = await _userManager.FindByIdAsync(id.ToString());
        if (userEf == null) return null;
        var result = await _userManager.AddToRoleAsync(userEf, role);
        if (!result.Succeeded) return null;
        var roles = await _userManager.GetRolesAsync(userEf);
        return new UserRolesResponse(userEf, roles);
    }

    public async Task<UserRolesResponse?> DeleteUserRolesAsync(Guid id, string role)
    {
        var userEf = await _userManager.FindByIdAsync(id.ToString());
        if (userEf == null) return null;
        var result = await _userManager.RemoveFromRoleAsync(userEf, role);
        if (!result.Succeeded) return null;
        var roles = await _userManager.GetRolesAsync(userEf);
        return new UserRolesResponse(userEf, roles);
    }

    public string GetCurrentUsername()
    {
        var nameId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultNameClaimType);

        return nameId != null
            ? nameId.Value.ToLower()
            : _httpContextAccessor.HttpContext.User.Identity?.Name?.ToLower();
    }
}