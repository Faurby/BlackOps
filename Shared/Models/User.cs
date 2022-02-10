using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MiniTwit.Shared;
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Name")]
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    [BsonRepresentation(BsonType.ObjectId)]
    public HashSet<string>? Follows { get; set; } = new HashSet<string>();
}