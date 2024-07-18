using WebApi.Domain.Enums;

namespace WebApi.ViewModels;

public class UserViewModel
{
    public string Username { get; set; }
    public UserRole Role { get; set; }
}