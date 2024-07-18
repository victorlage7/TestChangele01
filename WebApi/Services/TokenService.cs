using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApi.Domain.Entities;
using WebApi.Domain.Interfaces.Repositories;
using WebApi.Dtos;
using WebApi.Interfaces;

namespace WebApi.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    
    public TokenService(IConfiguration configuration, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }
    public async Task<string> GetToken(UserLoginDto userLoginDto)
    {
         var user = await _userRepository.GetAsync(username: userLoginDto.Username, password: userLoginDto.Password);
         if( user is null)
            return string.Empty;
         
         var tokenHandler = new JwtSecurityTokenHandler();
         var securityKey = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretJwt"));
        
         var tokenDescriptor = new SecurityTokenDescriptor
         {
             Subject = new ClaimsIdentity(new Claim[]
             {
                 new Claim(ClaimTypes.Name, user.Username),
                 new Claim(ClaimTypes.Role, (user.Role.ToString()))
             }),
             Expires = DateTime.UtcNow.AddHours(2),
             SigningCredentials = new SigningCredentials(
                 new SymmetricSecurityKey(securityKey), 
                 SecurityAlgorithms.HmacSha256Signature)
         };
        
         var token = tokenHandler.CreateToken(tokenDescriptor);
         return tokenHandler.WriteToken(token);
    }
    
}