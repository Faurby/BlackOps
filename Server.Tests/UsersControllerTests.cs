using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;

namespace Server.Tests;

public class UsersControllerTests
{

    private readonly Mock<IMongoCollection<User>> _usersCollection;
    
    private readonly Mock<IOptions<MiniTwitDatabaseSettings>> _options;
    private readonly Mock<IMongoClient> _client;
    private readonly Mock<IMongoDatabase> _db;


    public UsersControllerTests()
    {
        _options = new Mock<IOptions<MiniTwitDatabaseSettings>>();
        _client = new Mock<IMongoClient>();
        _db = new Mock<IMongoDatabase>();
        var settings = new MiniTwitDatabaseSettings() {ConnectionString = "mongodb://tes123", DatabaseName = "TestDB"};
        _options.Setup(s => s.Value).Returns(settings);
        _client.Setup(c => c.GetDatabase(_options.Object.Value.DatabaseName, null)).Returns(_db.Object);
        
    }

    [Fact]
    public async Task Get_returns_papers_from_collectionAsync()
    {
        var expected = new List<User>();
        var repository = new Mock<IUsersService>();
        repository.Setup(s => s.GetAsync()).ReturnsAsync(expected);
        var controller = new UsersController(repository.Object);

        var actual = await controller.Get(); 
        Assert.Equal(expected, actual);
    }
    [Fact]
    public async Task Get_given_existing_user_returns_user()
    {
        var expected = new User 
        {
            Id = "1", 
            UserName = "test123", 
            Password = "SomePassword", 
            PasswordSalt = "", 
            Email = "test.test@gmail.com", 
            Follows = new HashSet<string>(), 
            Followers = new HashSet<string>()
        };

        var repository = new Mock<IUsersService>();
        
        repository.Setup(s => s.GetAsync("1")).ReturnsAsync(expected);
        var controller = new UsersController(repository.Object);   

        var actual = await controller.Get("1");

        Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual.Value));
    }
}