using Core.Enums;

namespace Core.Dtos;

public class UserDto
{
    public string Username { get; set; }
    public UserRole Role { get; set; }
}