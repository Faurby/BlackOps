public interface IMessagesService
{
    Task<List<Message>> GetAsync();
    Task<Message?> GetAsync(string id);
    Task<List<Message>?> GetMessageFromUserIDAsync(string userID);
    Task<Status> CreateAsync(Message newMessage);
    Task UpdateAsync(string id, Message updatedMessage);
    Task RemoveAsync(string id);
    Task<List<Message>?> GetMessagesFromFollowing(String userID);
}