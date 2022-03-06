public class MiniTwitDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string UsersCollectionName { get; set; } = null!;
    public string MessagesCollectionName { get; set; } = null!;
    public string LatestCollectionName { get; set; } = null!;
}