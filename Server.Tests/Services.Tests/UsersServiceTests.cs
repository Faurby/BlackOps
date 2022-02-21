using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mongo2Go;
using Moq;

namespace Server.Tests;

public class UsersServiceTests
{
    private IMongoCollection<User> _collection;
    private IUsersService _usersService;
    public UsersServiceTests()
    {
        var _runner = MongoDbRunner.Start();

        var client = new MongoClient(_runner.ConnectionString);
        var database = client.GetDatabase("MiniTwit");
        _collection = database.GetCollection<User>("Users");

        var moqOptions = new MiniTwitDatabaseSettings() {UsersCollectionName = "Users"};
        IOptions<MiniTwitDatabaseSettings> options = Options.Create<MiniTwitDatabaseSettings>(moqOptions);

        _usersService = new UsersService(options, database);

        var user1 = new User()
        {
            UserName = "2cant",
            Password = "u1bwRX5aC5IvhSBksPjBwnEwmrEZ6jTWbJmO7ZtV1OSjqz0yv1OHKD0FJQeXpwM6H00dFGvykDVeQkjdcMm6Tu",
            PasswordSalt = "9PP9fEheyvtIkayhIPZ1eAcEdY/CeR7+UycrMK4IXn+jnpPfEA/OydP5isl2sCewF2rRTmmU1eeS77ujlcJIWiX0MZB1Ew==",
            Email = "anlf@itu.dk",
        };

        _collection.InsertOne(user1);
    }

    [Fact]
    public async Task GetAsync_returns_all_users()
    {
        // Arrange
        var user1 = new User()
        {
            UserName = "2cant",
            Password = "u1bwRX5aC5IvhSBksPjBwnEwmrEZ6jTWbJmO7ZtV1OSjqz0yv1OHKD0FJQeXpwM6H00dFGvykDVeQkjdcMm6Tu",
            PasswordSalt = "9PP9fEheyvtIkayhIPZ1eAcEdY/CeR7+UycrMK4IXn+jnpPfEA/OydP5isl2sCewF2rRTmmU1eeS77ujlcJIWiX0MZB1Ew==",
            Email = "anlf@itu.dk",
        };

        var expected = new List<User>() { user1 };
        // Act
        var actual = await _usersService.GetAsync();

        // Assert
        await AssertInListNoID(expected, actual);
    }

    public async Task AssertInListNoID(List<User> expected, List<User> actual)
    {
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected.ElementAt(i).Email, actual.ElementAt(i).Email);
            Assert.Equal(expected.ElementAt(i).UserName, actual.ElementAt(i).UserName);
            Assert.Equal(expected.ElementAt(i).Password, actual.ElementAt(i).Password);
        }
    }
}
