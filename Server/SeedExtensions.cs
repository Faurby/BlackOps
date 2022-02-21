using Microsoft.Extensions.Options;
using MiniTwit.Shared;
using MongoDB.Driver;

namespace MyApp.Server;

// DOES NOT WORK :(
public static class SeedExtensions
{
    private static IMongoCollection<User>? _usersCollection;
    private static IMongoCollection<Message>? _messagesCollection;
    private static IOptions<MiniTwitDatabaseSettings>? miniTwitDatabaseSettings;

    public static IHost Seed(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var mongoClient = new MongoClient(miniTwitDatabaseSettings!.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(miniTwitDatabaseSettings.Value.DatabaseName);

            mongoDatabase.DropCollection("Users");
            mongoDatabase.DropCollection("Messages");

            mongoDatabase.CreateCollection("Users");
            mongoDatabase.CreateCollection("Messages");


            _usersCollection = mongoDatabase.GetCollection<User>(miniTwitDatabaseSettings.Value.UsersCollectionName);
            _messagesCollection = mongoDatabase.GetCollection<Message>(miniTwitDatabaseSettings.Value.MessagesCollectionName);

            _usersCollection.InsertMany(GenerateUsers());
            _messagesCollection.InsertMany(GenerateMessages());

        }
        return host;
    }

    public static List<User> GenerateUsers()
    {
        var user1 = new User { UserName = "Lasse", Password = "1423" };
        var user2 = new User { UserName = "Anton", Password = "1342" };
        var user3 = new User { UserName = "Klaus", Password = "1415223" };
        var user4 = new User { UserName = "Carsten", Password = "1425553" };

        return new List<User> { user1, user2, user3, user4 };
    }

    public static List<User> GetUsers()
    {
        return _usersCollection.Find(_ => true).ToList();
    }

    public static List<Message> GenerateMessages()
    {
        var messageList = new List<Message>();

        foreach (var user in GetUsers())
        {
            messageList.Add(new Message { AuthorID = user.Id!, AuthorName = user.UserName, Text = GenerateText()});
        }
        return messageList;
    }

    public static string GenerateText()
    {
        Random r = new Random();
        var list = new List<string> 
            {
                "Hej med dig",
                "Hvordan går det",
                "Jeg kan godt lide kage",
                "Jeg går på ITU",
                "Måske måske"
            };

        return list.ElementAt(r.Next(0, list.Count-1));
    }
}