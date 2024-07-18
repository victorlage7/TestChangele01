using WebApi.Domain.Entities;
using WebApi.Dtos;

namespace WebApi.Interfaces;

public interface ITokenService
{
    public Task<string> GetToken(UserLoginDto userLoginDto); 
}