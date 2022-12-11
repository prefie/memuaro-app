namespace Rememory.WebApi.Dtos.User;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? PhotoUrl { get; set; }
    public IEnumerable<string>? Roles { get; set; }
}