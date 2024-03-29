﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Rememory.Persistance.Models;

namespace Rememory.Persistance.Entities;

public class Question : IDatabaseEntity
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public Guid UserId { get; set; }
    public Guid GlobalQuestionId { get; set; }
    public Guid CategoryId { get; set; }
    public string? Answer { get; set; }
    
    [JsonConverter(typeof(StringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Status Status { get; set; }
}