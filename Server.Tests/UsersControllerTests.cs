namespace Server.Tests;

public class UsersControllerTests
{
    [Fact]
    public void Get_returns_papers_from_collection()
    {
        var options = new IOptions<MiniTwitDatabaseSettings>();
        var userService = new UsersService(options);
        var controller = new UsersController(userService);
       
        var expected = userService._usersCollection;
        var actual = controller.GetAsync();

        Assert.Equal(expected, actual);


    }
}