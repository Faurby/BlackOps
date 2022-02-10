using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MiniTwit.Shared;
public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string AuthorID { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    [MaxLength(280)]
    public string Text { get; set; } = null!;
}