using Core.Dtos;
using Core.Entities;
using Messaging.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Text.Json;
using WebApi.Domain.Interfaces.Repositories;

namespace WebApi.Controllers;

[Route("api/[controller]/")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    private readonly IRabbitMqService _rabbitMqService;

    private readonly IConfiguration _configuration;

    public UserController(IUserRepository userRepository, IConfiguration configuration, IRabbitMqService rabbitMqService)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _rabbitMqService = rabbitMqService;
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
        User userResponse = null;
        string urlApi = "https://localhost:7000/api/user"+ "?username="+user.Username;
        var client = new HttpClient();
        var response = await client.GetAsync(urlApi);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var conteudo = await response.Content.ReadAsStringAsync();
            userResponse = JsonSerializer.Deserialize<User>(conteudo);
        }
        else
        {
            return BadRequest("Tente novamente mais tarde.");
        }

        if (userResponse != null)
        {
            return BadRequest("Usuário já existe.");
        }
        else 
        {
            using var connection = _rabbitMqService.CreateChannel();

            using (var model = connection.CreateModel())
            {
                model.QueueDeclare(
                queue: _configuration.GetSection("RabbitMqConfiguration").GetValue<string>("QueueNameCreateUser"),
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize<User>(user));

                model.BasicPublish(exchange: "",
                             routingKey: _configuration.GetSection("RabbitMqConfiguration").GetValue<string>("QueueNameCreateUser"),
                             mandatory: false,
                             basicProperties: null,
                             body: body
                             );
            }
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