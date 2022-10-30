using Memuaro.Persistance.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Memuaro.Persistance.Entities;

public class Question : IDatabaseEntity
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public Guid UserId { get; set; }
    public string? Answer { get; set; }
    
    [JsonConverter(typeof(StringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Status Status { get; set; }
}