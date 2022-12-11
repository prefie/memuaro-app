using MongoDB.Bson.Serialization.Attributes;

namespace Rememory.Persistance.Entities;

public interface IDatabaseEntity
{
    [BsonId]
    public Guid Id { get; set; }
}