namespace MiniTwit.Server;


public class MessagesService : IMessagesService
{
    private readonly IMongoCollection<Message> _messagesCollection;
    private readonly IMongoCollection<User> _usersCollection;
    private Counter _msgCounter = Metrics.CreateCounter("Message_Counter", "Counts the amount of messages added to the database");

    public MessagesService(IOptions<MiniTwitDatabaseSettings> miniTwitDatabaseSettings, IMongoDatabase mongoDatabase)
    {
        // var _mongoDatabase = mongoClient.GetDatabase(miniTwitDatabaseSettings.Value.DatabaseName);

        _messagesCollection = mongoDatabase.GetCollection<Message>(miniTwitDatabaseSettings.Value.MessagesCollectionName);
        _usersCollection = mongoDatabase.GetCollection<User>(miniTwitDatabaseSettings.Value.UsersCollectionName);

        _msgCounter.IncTo(GetAsync().Result.Count());
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
        _msgCounter.Inc();
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
    public async Task<VirtualizedResponse<Message>> GetVirtualizedAsync(int startIndex, int pageSize, DateTime openPageTime)
    {
        // print timestamp of lastest message
        var latestMessage = await _messagesCollection.Find(_ => true).SortByDescending(x => x.Timestamp).Limit(1).FirstOrDefaultAsync();
        //Console.WriteLine("-----");
        //Console.WriteLine("Latest message: " + latestMessage.Timestamp);
        //Console.WriteLine("Open time: " + openPageTime);
        //Console.WriteLine(latestMessage.Timestamp < openPageTime);


        var filterBuilder1 = Builders<Message>.Filter;
        var filter1 = filterBuilder1.Lt(x => x.Timestamp, openPageTime);

        var messages = await _messagesCollection
            .Find(filter1)
            //.Find(_ => true)
            .SortByDescending(m => m.Timestamp)
            .Skip(startIndex)
            .Limit(pageSize)
            .ToListAsync();

        //System.Console.WriteLine("Returning: " + messages.Count);
        return new VirtualizedResponse<Message>(){Items = messages, Size = startIndex + messages.Count+5};
    }

    public async Task<int> GetNewMessageCountAsync(DateTime openPageTime)
    {
        var filterBuilder1 = Builders<Message>.Filter;
        var filter1 = filterBuilder1.Gt(x => x.Timestamp, openPageTime);

        //get messages later than openPageTime
        var messages = await _messagesCollection
            .Find(filter1)
            .ToListAsync();

        return messages.Count;
    }
}