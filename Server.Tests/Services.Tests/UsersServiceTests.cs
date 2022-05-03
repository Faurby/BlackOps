using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
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

        var moqOptions = new MiniTwitDatabaseSettings() { UsersCollectionName = "Users" };
        IOptions<MiniTwitDatabaseSettings> options = Options.Create<MiniTwitDatabaseSettings>(moqOptions);

        _usersService = new UsersService(options, database);

        var user1 = new User()
        {
            UserName = "2cant",
            Password = "u1bwRX5aC5IvhSBksPjBwnEwmrEZ6jTWbJmO7ZtV1OSjqz0yv1OHKD0FJQeXpwM6H00dFGvykDVeQkjdcMm6Tu",
            PasswordSalt = "9PP9fEheyvtIkayhIPZ1eAcEdY/CeR7+UycrMK4IXn+jnpPfEA/OydP5isl2sCewF2rRTmmU1eeS77ujlcJIWiX0MZB1Ew==",
            Email = "anlf@itu.dk",
            Follows = new HashSet<string>() { "123456789123456789123456" }
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
            Follows = new HashSet<string>() { "123456789123456789123456" }

        };

        var userDTO1 = new UserDTO(user1.Id, user1.UserName, user1.Email, user1.Follows,
            user1.Followers)
        ;

        var expected = new List<UserDTO>() { userDTO1 };
        // Act
        var actual = await _usersService.GetAsync();

        // Assert
        await AssertInListNoID(expected, actual);
    }

    public async Task AssertInListNoID(List<UserDTO> expected, List<UserDTO> actual)
    {
        for (int i = 0; i < expected.Count; i++)
        {
            await AssertTwoUsersIgnoreId(expected.ElementAt(i), actual.ElementAt(i));
        }
    }

    public async Task AssertTwoUsersIgnoreId(UserDTO expected, UserDTO actual)
    {
        Assert.Equal(expected.UserName, actual.UserName);
        Assert.Equal(expected.Email, actual.Email);
        // Assert.Equal(expected.Password, actual.Password);
        // Assert.Equal(expected.PasswordSalt, actual.PasswordSalt);

        if (!expected.Followers.IsNullOrEmpty() && !actual.Followers.IsNullOrEmpty())
        {
            for (int i = 0; i < expected.Follows.Count; i++)
            {

                var expectedFollowers = expected.Followers.ElementAt(i);
                var actualFollowers = actual.Followers.Where(s => s == expectedFollowers).FirstOrDefault();

                Assert.Equal(expectedFollowers, actualFollowers);
            }
        }

        if (!expected.Follows.IsNullOrEmpty() && !actual.Follows.IsNullOrEmpty())
        {
            for (int i = 0; i < expected.Followers.Count; i++)
            {
                var expectedFollows = expected.Follows.ElementAt(i);
                var actualFollows = actual.Follows.Where(s => s == expectedFollows).FirstOrDefault();

                Assert.Equal(expectedFollows, actualFollows);
            }
        }
    }
}
