using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Rememory.Persistance.Models;

namespace Rememory.Persistance.Entities;

public class User : IDatabaseEntity
{
    [BsonConstructor]
    public User(Guid id, string email, string? name, string? photoUrl, HashSet<Role>? roles)
    {
        Id = id;
        Email = email;
        Name = name;
        PhotoUrl = photoUrl;
        Roles = roles;
    }

    public Guid Id { get; set; }
    public string Email { get; set; }
    public string? Name { get; set; }
    public string? RefreshToken { get; set; }
    public string? PhotoUrl { get; set; }
    
    [JsonConverter(typeof(StringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public HashSet<Role>? Roles { get; set; }
}