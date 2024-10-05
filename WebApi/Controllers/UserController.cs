using Core.Dtos;
using Core.Entities;
using Messaging.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
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
        User myObject = null;
        string urlApi = "https://localhost:7000/api/user/"+user.Username;
        var jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        var client = new HttpClient();
        var response = await client.GetAsync(urlApi);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return BadRequest("Usuário já existe.");
        }
        else if (!response.IsSuccessStatusCode)
        {
            return BadRequest("Tente novamente mais tarde.");
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
        return Ok("Usuário criado com sucesso!");
        }
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
        User myObject = null;
        string urlApi = "https://localhost:7000/api/user/" + user.Username;
        var jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        var client = new HttpClient();
        var response = await client.GetAsync(urlApi);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var connection = _rabbitMqService.CreateChannel();

            using (var model = connection.CreateModel())
            {
                model.QueueDeclare(
                queue: _configuration.GetSection("RabbitMqConfiguration").GetValue<string>("QueueNameUpdateUser"),
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize<User>(user));

                model.BasicPublish(exchange: "",
                             routingKey: _configuration.GetSection("RabbitMqConfiguration").GetValue<string>("QueueNameUpdateUser"),
                             mandatory: false,
                             basicProperties: null,
                             body: body
                             );
            }
            return Ok("Usuário Atualizado com sucesso!");
        }
        else if (!response.IsSuccessStatusCode)
        {
            return BadRequest("Tente novamente mais tarde.");
        }
        else if (response.StatusCode == HttpStatusCode.OK)
        {
            return NoContent();
        }
        return BadRequest("Tente novamente mais tarde.");

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

        User myObject = null;
        string urlApi = "https://localhost:7000/api/user/" + username;
        var jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        var client = new HttpClient();
        var response = await client.GetAsync(urlApi);

        if (!response.IsSuccessStatusCode)
        {
            return BadRequest("Tente novamente mais tarde.");
        }
        else if (response.StatusCode == HttpStatusCode.NoContent)
        {
            return NoContent();
        }
        else
        {
            var conteudo = await response.Content.ReadAsStringAsync();

            myObject = JsonSerializer.Deserialize<User>(conteudo, jsonOptions);

            using var connection = _rabbitMqService.CreateChannel();

            using (var model = connection.CreateModel())
            {
                model.QueueDeclare(
                queue: _configuration.GetSection("RabbitMqConfiguration").GetValue<string>("QueueNameDeleteUser"),
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(myObject));

                model.BasicPublish(exchange: "",
                             routingKey: _configuration.GetSection("RabbitMqConfiguration").GetValue<string>("QueueNameDeleteUser"),
                             mandatory: false,
                             basicProperties: null,
                             body: body
                             );
            }
            return Ok("Usuário deletado com sucesso!");
        }
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
        User user = null;
        string urlApi = "https://localhost:7000/api/user/"+username;
        var jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        var client = new HttpClient();
        var response = await client.GetAsync(urlApi);
        var conteudo = await response.Content.ReadAsStringAsync();
        user = JsonSerializer.Deserialize<User>(conteudo, jsonOptions);

        if (!response.IsSuccessStatusCode)
        {
            return BadRequest("Tente novamente mais tarde.");
        }

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

        List<User> users = new List<User>();

        string urlApi = "https://localhost:7000/api/user/";
        var jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        var client = new HttpClient();
        var response = await client.GetAsync(urlApi);

        var conteudo = await response.Content.ReadAsStringAsync();
        users = JsonSerializer.Deserialize<List<User>>(conteudo, jsonOptions);

        if (!response.IsSuccessStatusCode)
        {
            return BadRequest("Tente novamente mais tarde.");
        }

        return Ok(users);
    }
    
}