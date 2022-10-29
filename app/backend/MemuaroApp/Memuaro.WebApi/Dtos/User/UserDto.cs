using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Memuaro.WebApi.Dtos.User;

[JsonConverter(typeof(StringEnumConverter))]
public class UserDto
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? PhotoUrl { get; set; }
    public IEnumerable<string>? Roles { get; set; }
}