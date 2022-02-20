using Microsoft.Extensions.Options;
using MiniTwit.Shared;
using MongoDB.Driver;

namespace MiniTwit.Server;

public class MessagesService
{
    private readonly IMongoCollection<Message> _messagesCollection;
    private readonly IMongoCollection<User> _usersCollection;

    public MessagesService(IOptions<MiniTwitDatabaseSettings> miniTwitDatabaseSettings, IMongoDatabase mongoDatabase)
    {
        // var _mongoDatabase = mongoClient.GetDatabase(miniTwitDatabaseSettings.Value.DatabaseName);

        _messagesCollection = mongoDatabase.GetCollection<Message>(miniTwitDatabaseSettings.Value.MessagesCollectionName);
        _usersCollection = mongoDatabase.GetCollection<User>(miniTwitDatabaseSettings.Value.UsersCollectionName);
    }

    public async Task<List<Message>> GetAsync()
    {
        var messages = await _messagesCollection.Find(_ => true).ToListAsync();

        // TODO: Do this smarter haha. Make DB automatically insert messages in descending order of timestamps
        messages.Reverse();

        return messages;
    }

    public async Task<Message?> GetAsync(string id) =>
        await _messagesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<List<Message>?> GetMessageFromUserIDAsync(string userID) =>
        await _messagesCollection.Find(x => x.AuthorID == userID).ToListAsync();

    public async Task<Status> CreateAsync(Message newMessage)
    {
        await _messagesCollection.InsertOneAsync(newMessage);
        return Status.Created;
    }

    public async Task UpdateAsync(string id, Message updatedMessage) =>
        await _messagesCollection.ReplaceOneAsync(x => x.Id == id, updatedMessage);

    public async Task RemoveAsync(string id) =>
        await _messagesCollection.DeleteOneAsync(x => x.Id == id);

    public async Task<List<Message>?> GetMessagesFromFollowing(String userID)
    {
        var user = await _usersCollection.Find(x => x.Id == userID).FirstOrDefaultAsync();
        return await _messagesCollection.Find(x => user.Follows.Contains(x.AuthorID)).ToListAsync();
    }

}