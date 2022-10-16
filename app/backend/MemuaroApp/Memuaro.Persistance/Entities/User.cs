using Memuaro.Persistance.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Memuaro.Persistance.Entities;

public class User : IDatabaseEntity
{
    public User(Guid id, string email): this(id, email, null)
    {
    }

    [BsonConstructor]
    public User(Guid id, string email, HashSet<Role>? roles)
    {
        Id = id;
        Email = email;
        Roles = roles;
    }
    

    public Guid Id { get; set; }
    public string Email { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public HashSet<Role>? Roles { get; set; }
}