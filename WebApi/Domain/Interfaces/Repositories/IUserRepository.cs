using Core.Dtos;
using Core.Entities;

namespace WebApi.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<UserDto?> GetAsync(string username);
    Task<User?> GetAsync(string username, string password);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<bool> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(string username);  
}