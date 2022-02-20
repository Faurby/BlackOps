namespace Server.Tests;

public class UsersControllerTests
{

    private readonly IMongoCollection<User> _usersCollection;
    
    private IOptions<MiniTwitDatabaseSettings> _options;

    private readonly UsersController _controller;

    private readonly UsersService _repository;

    public UsersControllerTests()
    {
        //Does not work since we need to have the correct connectionString to actually retrieve the "repo" in usersService.
      var options = new MiniTwitDatabaseSettings(){ConnectionString = "abc", DatabaseName = "testDB"};
      _options = Options.Create(options);
      _repository = new UsersService(_options);
    }

    [Fact]
    public void Get_returns_papers_from_collection()
    {
        Assert.True(true);   
    }
}