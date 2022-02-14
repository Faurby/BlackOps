using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public string? PasswordSalt { get; set; } // For ID and Password salt we should get rid of th nullable property by using DTOs
    [EmailAddress]
    public string Email { get; set; } = null!;

    [BsonRepresentation(BsonType.ObjectId)]
    public HashSet<string> Follows { get; set; } = new HashSet<string>();
}