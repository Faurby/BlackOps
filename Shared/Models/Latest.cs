using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Latest
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public int latest { get; set; }
}
