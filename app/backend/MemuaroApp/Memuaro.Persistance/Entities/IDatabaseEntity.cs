using MongoDB.Bson.Serialization.Attributes;

namespace Memuaro.Persistance.Entities;

public interface IDatabaseEntity
{
    [BsonId]
    public Guid Id { get; set; }
}