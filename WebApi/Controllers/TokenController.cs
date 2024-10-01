using Core.Dtos;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApi.Interfaces;

namespace WebApi.Controllers;

[Route("api/[controller]/")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateToken([FromBody] UserLoginDto userLoginDto) 
    {
        string urlApi = "https://localhost:7000/api/user/" + userLoginDto.Username;
        var jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        var client = new HttpClient();
        var response = await client.GetAsync(urlApi);
        var conteudo = await response.Content.ReadAsStringAsync();

        User user = JsonSerializer.Deserialize<User>(conteudo, jsonOptions);

        var token = _tokenService.GetToken(user); 
        if (!string.IsNullOrEmpty(token))
        {
            return Ok(token);
        }

        return Unauthorized();
    }
}