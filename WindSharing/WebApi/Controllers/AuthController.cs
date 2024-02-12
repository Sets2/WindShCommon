using Core.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using WebApi.Auth;
using WebApi.Models.Auth;
using Core.Abstractions.Repositories;
using WebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class AuthController : ControllerBase
{
    private readonly IUserRepositoryAuth _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IRepository<UserWind> _userWindRepository;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUserRepositoryAuth userRepository,
        ITokenService tokenService,
        IRepository<UserWind> userWindRepository,
        ILogger<AuthController> logger)
    {
        _userRepository = userRepository;
        _userWindRepository = userWindRepository;
        _tokenService = tokenService;
        _logger = logger;
    }

    /// <summary>
    /// Представляет токен доступа на основании <paramref name="password"/> и <paramref name="username"/> либо <paramref name="id"/>.
    /// Если указаны оба параметра <paramref name="username"/> и <paramref name="id"/> поиск будет идти по <paramref name="id"/>
    /// </summary>
    /// <param name="id">Id пользователя, пароль у которого уже установлен, например <example>493fbfb1-2608-4c39-be19-906dfa4edde7</example></param>
    /// <param name="username">Пользователь, пароль у которого уже установлен, например <example>Petrov</example></param>
    /// <param name="password">Действующий пароль, например <example>Uu_123</example></param>
    /// <returns>
    /// The <see cref="Task"/>that represents the asynchronous operation, containing the <see cref="ActionResult"/> of the operation.
    /// </returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult> GetUserToken([FromQuery] Guid? id, [FromQuery] string? username, [FromQuery] string password)
    {
        if (id == null & username == null) return BadRequest("Должен быть указан хотя бы один из параметров Id или UserName");
        var user = new UserAuthDto() { Id = id, UserName = username, Password = password };

        var userAuth = await _userRepository.GetUserForToken(user);
        if (userAuth != null)
        {
            var token = _tokenService.BuildToken(userAuth);
            return Ok(token);
        }
        else return Unauthorized();
    }

    /// <summary>
    /// Представляет токен доступа на основании <paramref name="password"/> и <paramref name="username"/>
    /// </summary>
    /// <param name="username">Пользователь, пароль у которого уже установлен, например <example>Petrov</example></param>
    /// <param name="password">Действующий пароль, например <example>Uu_123</example></param>
    /// <returns>
    /// The <see cref="Task"/>that represents the asynchronous operation, containing the <see cref="ActionResult"/> of the operation.
    /// </returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<UserWindWithToken>> GetUserToken(LoginPassswordDto dto)
    {
        UserWindWithToken response = new UserWindWithToken();
        UserAuthDto user = new(dto.UserName, dto.Password);

        var userWindAuth = await _userRepository.GetUserWindAuthForToken(user);
        if (userWindAuth != null)
        {
            if (userWindAuth.UserWind.IsActive?? false)
            {
                response.Token = _tokenService.BuildToken(userWindAuth.UserAuth);
                response.UserWind = new UserWindResponse(userWindAuth.UserWind, userWindAuth.UserAuth.Roles);
                _logger.LogInformation($"Вход пользователя {dto.UserName}");
                return Ok(response);
            }
            else
            {
                _logger.LogError($"Попытка входа отключенного пользователя {dto.UserName}");
                response.Error = "Вход запрещен";
            }
        }
        else
        {
            response.Error = "Неверный логин или пароль";
        }
        return Ok(response);
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserWindWithToken>> GetMe()
    {
        UserWindWithToken response = new UserWindWithToken();
        var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return NotFound("В клейме отсутствует идентификатор");

        var userWindAuth = await _userRepository.GetUserWindAuthForRefreshToken(userId);
        if (userWindAuth != null)
        {
            response.Token = _tokenService.BuildToken(userWindAuth.UserAuth);
            response.UserWind = new UserWindResponse(userWindAuth.UserWind, userWindAuth.UserAuth.Roles);
            return Ok(response);
        }
        else
        {
            response.Error = "Ошибка поиска пользователя по данным клейме токена";
        }

        return Ok(response);
    }

    /// <summary>
    /// Создает <paramref name="password"/> пользователю, определенному <paramref name="username"/> либо <paramref name="id"/> только если пользователь
    /// еще не имеет установленный пароль.
    /// Если указаны оба параметра <paramref name="username"/> и <paramref name="id"/> поиск будет идти по <paramref name="id"/>
    /// </summary>
    /// <param name="id">Id пользователя, пароль у которого еще не установлен, например <example>48CD68E0-475C-44FB-BC3C-B40504B7CDA2</example></param>
    /// <param name="username">Пользователь, пароль у которого еще не установлен, например <example>Sidorov</example></param>
    /// <param name="password">Пароль для установки, например <example>Ss_123</example></param>
    /// <returns>
    /// The <see cref="Task"/>that represents the asynchronous operation, containing the <see cref="ActionResult"/> of the operation.
    /// </returns>
    [Authorize(Policy = AdminOrOwnerRequirement.Name)]
    [HttpPost]
    public async Task<ActionResult> CreatePassword([FromQuery] Guid? id, [FromQuery] string? username, [FromQuery] string password)
    {
        if (id == null & username == null)
            return BadRequest("Должен быть указан хотя бы один из параметров Id или UserName");
        var user = new UserAuthDto() { Id = id, UserName = username, Password = password };

        var userAuth = await _userRepository.CreateUserPassword(user);
        if (userAuth != null)
        {
            return Ok();
        }
        else return Unauthorized();
    }

    /// <summary>
    /// Создает <paramref name="username"/> пользователя, с паролем <paramref name="password"/> и эл.почтой <paramref name="email"/>  
    /// </summary>
    /// <param name="username">Имя пользователя<example>Novosel</example></param>
    /// <param name="password">Пароль для установки (должны соблюдаться минимальные требования Identity), например <example>Nn_123</example></param>
    /// <param name="email">email пользователя, например <example>mymail@mail.ru</example></param>
    /// <returns>
    /// The <see cref="Task"/>that represents the asynchronous operation, containing the <see cref="ActionResult"/> of the operation.
    /// </returns>
    [Authorize(Roles = "admin")]
    [HttpPost("user")]
    [AllowAnonymous]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserWindDto user)
    {
        var response = await _userRepository.CreateUsersWithToken(user.UserName, user.Password, user.Email, "user");

        return Ok(response);
    }

    /// <summary>
    /// Обновляет <paramref name="password"/> пользователю, определенному <paramref name="username"/> либо <paramref name="id"/> только если пользователь
    /// уже имеет пароль.
    /// Если указаны оба параметра <paramref name="username"/> и <paramref name="id"/> поиск будет идти по <paramref name="id"/>
    /// </summary>
    /// <param name="id">Id пользователя, пароль у которого уже установлен, например <example>493fbfb1-2608-4c39-be19-906dfa4edde7</example></param>
    /// <param name="username">Пользователь, пароль у которого уже установлен, например <example>Petrov</example></param>
    /// <param name="password">Действующий пароль, например <example>Uu_123</example></param>
    /// <param name="passwordNew">Пароль для установки, например <example>Uu_12345</example></param>
    /// <returns>
    /// The <see cref="Task"/>that represents the asynchronous operation, containing the <see cref="ActionResult"/> of the operation.
    /// </returns>
    [Authorize(Policy = OwnerRequirement.Name)]
    [HttpPut]
    public async Task<ActionResult> UpdatePassword([FromQuery] Guid? id, [FromQuery] string? username, [FromQuery] string password,
        [FromQuery] string passwordNew)
    {
        if (id == null & username == null)
            return BadRequest("Должен быть указан хотя бы один из параметров Id или UserName");
        var user = new UserAuthDto() { Id = id, UserName = username, Password = password };

        var userAuth = await _userRepository.UpdateUserPassword(user, passwordNew);
        if (userAuth != null)
        {
            return NoContent();
        }
        else return Unauthorized();
    }

    /// <summary>
    /// Удаляет пароль у пользователя, определенного <paramref name="username"/> либо <paramref name="id"/>. 
    /// Если указаны оба параметра <paramref name="username"/> и <paramref name="id"/> поиск будет идти по <paramref name="id"/>
    /// </summary>
    /// <param name="id">Id пользователя, например <example>493fbfb1-2608-4c39-be19-906dfa4edde7</example></param>
    /// <param name="username">Пользователь, например <example>Petrov</example></param>
    /// <returns>
    /// The <see cref="Task"/>that represents the asynchronous operation, containing the <see cref="ActionResult"/> of the operation.
    /// </returns>
    [Authorize(Policy = AdminOrOwnerRequirement.Name)]
    [HttpDelete]
    public async Task<ActionResult> DeletePassword([FromQuery] Guid? id, [FromQuery] string? username)
    {
        if (id == null & username == null)
            return BadRequest("Должен быть указан хотя бы один из параметров Id или UserName");
        var user = new UserAuthDto() { Id = id, UserName = username };
        var userAuth = await _userRepository.DeleteUserPassword(user);
        if (userAuth != null)
        {
            return NoContent();
        }
        else return Unauthorized();
    }
}