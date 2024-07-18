using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;
using WebApi.Domain.Interfaces.Repositories;
using WebApi.Dtos;
using WebApi.Infrastructure.Context;

namespace WebApi.Infrastructure.Repositories;

/// <summary>
/// Repositorio de usuários
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly TechChallenge1DbContext _context;
    
    /// <summary>
    /// Ciando uma nova instância de <see cref="UserRepository"/>.
    /// </summary>
    /// <param name="context">DbContext</param>
    public UserRepository(TechChallenge1DbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Retorna um usuário pelo username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<UserDto?> GetAsync(string username)
    {
        return await _context.Users
            .Where(u => u.Username.Equals(username))
            .Select(x => new UserDto
            {
                Username = x.Username,
                Role = x.Role 
            })
            .FirstOrDefaultAsync();
    }
    
    /// <summary>
    /// Retorna um usuário pelo username e password.
    /// </summary>
    /// <param name="username">Usuário</param>
    /// <param name="password">Senha</param>
    /// <returns>Usuario</returns>
    public async Task<User?> GetAsync(string username, string password)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(username) && u.Password.Equals(password));
    }
    
    /// <summary>
    /// Retorna todos os usuários.
    /// </summary>
    /// <returns>Lista de usuários</returns>
    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        return await _context.Users
            .Select(x => new UserDto
            {
                Username = x.Username,
                Role = x.Role 
            })
            .ToListAsync();
    }
    
    /// <summary>
    /// Adiciona um novo usuário.
    /// </summary>
    /// <param name="user">Usuário</param>
    public async Task<bool> AddAsync(User user)
    {
        if (await _context.Users.AnyAsync(u => u.Username.Equals(user.Username)))
            return false;
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return true;
    }
    
    /// <summary>
    /// Atualiza um usuário.
    /// </summary>
    /// <param name="user">Usuário</param>
    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Delete um usuário.
    /// </summary>
    /// <param name="username">login do usuario</param>
    public async Task DeleteAsync(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(username));

        if (user is not null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    
    }
    
}