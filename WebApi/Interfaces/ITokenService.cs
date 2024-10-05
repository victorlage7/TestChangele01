using Core.Dtos;
using Core.Entities;

namespace WebApi.Interfaces;

public interface ITokenService
{
    public Task<string> GetToken(UserLoginDto userLoginDto);

    public string GetToken(User user);
}