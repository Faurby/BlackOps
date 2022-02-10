using Microsoft.Extensions.Options;
using MiniTwit.Shared;
using MongoDB.Driver;

namespace MiniTwit.Server;

public class MessagesService
{
    private readonly IMongoCollection<Message> _messagesCollection;

    public MessagesService(
        IOptions<MiniTwitDatabaseSettings> miniTwitDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            miniTwitDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            miniTwitDatabaseSettings.Value.DatabaseName);

        _messagesCollection = mongoDatabase.GetCollection<Message>(
            miniTwitDatabaseSettings.Value.MessagesCollectionName);
    }

    public async Task<List<Message>> GetAsync() =>
        await _messagesCollection.Find(_ => true).ToListAsync();

    public async Task<Message?> GetAsync(string id) =>
        await _messagesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<List<Message>?> GetMessageFromUserIDAsync(string userID) =>
        await _messagesCollection.Find(x => x.AuthorID == userID).ToListAsync();

    public async Task CreateAsync(Message newMessage) =>
        await _messagesCollection.InsertOneAsync(newMessage);

    public async Task UpdateAsync(string id, Message updatedMessage) =>
        await _messagesCollection.ReplaceOneAsync(x => x.Id == id, updatedMessage);

    public async Task RemoveAsync(string id) =>
        await _messagesCollection.DeleteOneAsync(x => x.Id == id);
}