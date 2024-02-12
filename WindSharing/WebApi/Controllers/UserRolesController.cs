using Core.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Auth;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRepositoryAuth _userRepository;
        private readonly ILogger<UserRolesController> _logger;
        public UserRolesController(IUserRepositoryAuth userRepository, ILogger<UserRolesController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Возвращает все роли, имеющиеся в системе
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>that represents the asynchronous operation, containing the <see cref="ActionResult"/>
        /// of the operation.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetRolesAsync()
        {
            var result = await _userRepository.GetRolesAsync();
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Возвращает все роли пользователя, определенного <paramref name="id"/>
        /// </summary>
        /// <param name="id">Id пользователя, например <example>493fbfb1-2608-4c39-be19-906dfa4edde7</example></param>
        /// <returns>
        /// The <see cref="Task"/>that represents the asynchronous operation, containing the <see cref="ActionResult"/>
        /// of the operation.
        /// </returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserRolesResponse>> GetUserRolesAsync(Guid id)
        {
            var user = await _userRepository.GetUserRolesAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Добавляет роль <paramref name="role"/> пользователю, определенному <paramref name="id"/>
        /// </summary>
        /// <param name="id">Id пользователя, например <example>493fbfb1-2608-4c39-be19-906dfa4edde7</example></param>
        /// <param name="role">Пароль для установки, например <example>admin</example></param>
        /// <returns>
        /// The <see cref="Task"/>that represents the asynchronous operation, containing the <see cref="ActionResult"/>
        /// of the operation.
        /// </returns>	
        [HttpPost]
        public async Task<ActionResult<UserRolesResponse>> CreateUserRolesAsync(Guid id, string role)
        {
            var user = await _userRepository.CreateUserRolesAsync(id, role);
            if (user == null) return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Удаляет роль <paramref name="role"/> пользователю, определенному <paramref name="id"/>
        /// </summary>
        /// <param name="id">Id пользователя, например <example>493fbfb1-2608-4c39-be19-906dfa4edde7</example></param>
        /// <param name="role">Пароль для установки, например <example>user</example></param>
        /// <returns>
        /// The <see cref="Task"/>that represents the asynchronous operation, containing the <see cref="ActionResult"/>
        /// of the operation.
        /// </returns>		
        [HttpDelete]
        public async Task<ActionResult<UserRolesResponse>> DeleteUserRolesAsync(Guid id, string role)
        {
            var user = await _userRepository.DeleteUserRolesAsync(id, role);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}
