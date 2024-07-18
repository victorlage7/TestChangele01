using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
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
        var token = await _tokenService.GetToken(userLoginDto); 
        if (!string.IsNullOrEmpty(token))
        {
            return Ok(token);
        }

        return Unauthorized();
    }
}