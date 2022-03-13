namespace MiniTwit.Server;

public class LatestService : ILatestService
{
    private readonly IMongoCollection<Latest> _latestCollection;

    public LatestService(IOptions<MiniTwitDatabaseSettings> miniTwitDatabaseSettings, IMongoDatabase mongoDatabase)
    {
        _latestCollection = mongoDatabase.GetCollection<Latest>(miniTwitDatabaseSettings.Value.LatestCollectionName);
    }

    public async Task<LatestDTO> GetAsync()
    {
        var latestDB = (await (await _latestCollection.FindAsync(_ => true)).FirstOrDefaultAsync());

        if (latestDB == null)
        {
            return new LatestDTO {};
        }

        return new LatestDTO() { latest = latestDB.latest };
    }

    public async Task<Status> UpdateAsync(LatestDTO dto)
    {
        var latestFromDTO = dto.latest;

        var latestDB = await (await _latestCollection.FindAsync(_ => true)).FirstOrDefaultAsync();

        if (latestDB == null)
        {
            latestDB = new Latest() { latest = latestFromDTO };

            await _latestCollection.InsertOneAsync(latestDB);

            return Status.Created;
        }

        await _latestCollection.DeleteManyAsync(_ => true);

        latestDB = new Latest() { latest = latestFromDTO };

        await _latestCollection.InsertOneAsync(latestDB);

        return Status.Created;
    }
}