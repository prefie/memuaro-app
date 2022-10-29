using Memuaro.Persistance.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Memuaro.WebApi.Dtos.User;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? PhotoUrl { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public HashSet<Role>? Roles { get; set; }
}