using Memuaro.Persistance.Models;

namespace Memuaro.Auth.Dtos;

public class UserCredentials
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public HashSet<Role> Roles { get; set; }
}