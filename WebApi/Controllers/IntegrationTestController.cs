using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.Interfaces.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class IntegrationTestController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public IntegrationTestController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Adiciona um novo usuário.
        /// </summary>
        /// <param name="user">Usuário</param>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> AddUserAsync([FromBody] User user)
        {
            if (!await _userRepository.AddAsync(user))
            {
                return BadRequest("Usuário já existe.");
            }
            return Ok("Usuário criado com sucesso!");
        }

    }
}
