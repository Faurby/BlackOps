namespace MiniTwit.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public record UserDTO(string Id, string UserName, string Email, HashSet<String> Follows, HashSet<String> Followers);
public record CreateUserDTO {
    // I want to create a UserDTO class which can pass the User.cs objects around the application without passing along sensitive data like passwords.
    [BsonElement("Name")]
    public string? UserName { get; set; }
    [EmailAddress]
    public string? Email { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public HashSet<string>? Follows { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public HashSet<string>? Followers { get; set; }

}

public record UpdateUserDTO : CreateUserDTO
{
    public int Id {get; set;}
}