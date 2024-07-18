using WebApi.Domain.Enums;

namespace WebApi.Dtos;

public class UserDto
{
    public string Username { get; set; }
    public UserRole Role { get; set; }
}