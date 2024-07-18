using WebApi.Domain.Enums;

namespace WebApi.Domain.Entities;

public class User
{
    public User(string username, string password, UserRole role)
    {
        Username = username;
        Password = password;
        Role = role;
    }

    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }    
    public UserRole Role { get; set; }
}