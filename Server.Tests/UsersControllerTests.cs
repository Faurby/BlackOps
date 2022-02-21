using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;

namespace Server.Tests;

public class UsersControllerTests
{    
    // If you're looking for the previous constructor Caspar, its under notion. I did not see the reason for it to be here.

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