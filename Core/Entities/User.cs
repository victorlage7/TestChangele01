using Core.Enums;

namespace Core.Entities;

public class User
{
    public User()
    {
    }

    public User(string username, string password, UserRole role)
    {
        Username = username;
        Password = password;
        Role = role;
    }

    public User(Guid id, string username, string password, UserRole role)
    {
        Id = id;
        Username = username;
        Password = password;
        Role = role;
    }

    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }    
    public UserRole Role { get; set; }
}