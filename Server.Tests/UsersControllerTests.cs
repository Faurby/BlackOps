using Moq;

namespace Server.Tests;

public class UsersControllerTests
{

    private readonly Mock<IMongoCollection<User>> _usersCollection;
    
    private readonly Mock<IOptions<MiniTwitDatabaseSettings>> _options;
    private readonly Mock<IMongoClient> _client;
    private readonly Mock<IMongoDatabase> _db;

    private readonly UsersController _controller;

    private readonly UsersService _repository;

    public UsersControllerTests()
    {
        _options = new Mock<IOptions<MiniTwitDatabaseSettings>>();
        _client = new Mock<IMongoClient>();
        _db = new Mock<IMongoDatabase>();
        var settings = new MiniTwitDatabaseSettings() {ConnectionString = "mongodb://tes123", DatabaseName = "TestDB"};
        _options.Setup(s => s.Value).Returns(settings);
        _client.Setup(c => c.GetDatabase(_options.Object.Value.DatabaseName, null)).Returns(_db.Object);
        _repository = new UsersService(_options.Object, _db.Object);
        _controller = new UsersController(_repository);
    }

    [Fact]
    public void Get_returns_papers_from_collection()
    {
        
    }
}