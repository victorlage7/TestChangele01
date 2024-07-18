using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.Entities;
using WebApi.Domain.Interfaces.Repositories;
using WebApi.Dtos;

namespace WebApi.Controllers;

[Route("api/[controller]/")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
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
    
    /// <summary>
    /// Atualiza um usuário.
    /// </summary>
    /// <param name="user">Usuario</param>
    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> UpdateUserAsync([FromBody] User user)
    {
        await _userRepository.UpdateAsync(user);
        return Ok("Usuário atualizado com sucesso!");
    }
    
    /// <summary>
    /// Delete um usuário.
    /// </summary>
    /// <param name="username">Username</param>
    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> DeleteUserAsync(string username)
    {
        await _userRepository.DeleteAsync(username);
        return Ok("Usuário deletado com sucesso!");
    }
    
    /// <summary>
    /// Retorna um usuário pelo username.
    /// </summary>
    /// <param name="username">Username</param>
    [HttpGet("{username}")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> GetUserAsync(string username)
    {
        var user = await _userRepository.GetAsync(username);
        if (user is null)
            return NotFound("Usuário não encontrado.");
        
        return Ok(user);
    }
    
    /// <summary>
    /// Retorna todos os usuários.
    /// </summary>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(UserDto[]), 200)]
    [ProducesResponseType(typeof(string), 404)] 
    public async Task<IActionResult> GetUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        if (!users.Any())
            return NotFound("Nenhum usuário encontrado.");
        
        return Ok(users);
    }
    
}