using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using Newtonsoft.Json;

namespace Server.Tests;

public class UsersControllerTests
{
    // If you're looking for the previous constructor Caspar, its under notion. I did not see the reason for it to be here.

    [Fact]
    public async Task Get_returns_empty_from_collectionAsync()
    {
        var expected = new List<User>();
        var repository = new Mock<IUsersService>();
        repository.Setup(s => s.GetAsync()).ReturnsAsync(expected);
        var controller = new UsersController(repository.Object);

        var actual = await controller.Get();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Get_returns_users_from_collectionAsync()
    {
        var user1 = new User()
        {
            Email = "anlf@itu.dk",
            Id = "6213ca8fdd59699e46c43d4c",
            Password = "u1bwRX5aC5IvhSBksPjBwnEwmrEZ6jTWbJmO7ZtV1OSjqz0yv1OHKD0FJQeXpwM6H00dFGvykDVeQkjdcMm6Tu/zkBh+Zg==",
            PasswordSalt = "9PP9fEheyvtIkayhIPZ1eAcE…eeS77ujlcJIWiX0MZB1Ew=="
        };

        var user2 = new User()
        {
            Email = "anlf2@itu.dk",
            Id = "6213ca8fdd59699e46c43d4c",
            Password = "u1bwRX5aC5IvhSBksPjBwnEwmrEZ6jTWbJmO7ZtV1OSjqz0yv1OHKD0FJQeXpwM6H00dFGvykDVeQkjdcMm6Tu/zkBh+Zg==",
            PasswordSalt = "9PP9fEheyvtIkayhIPZ1eAcE…eeS77ujlcJIWiX0MZB1Ew=="
        };

        var user3 = new User()
        {
            Email = "anlf3@itu.dk",
            Id = "6213ca8fdd59699e46c43d4c",
            Password = "u1bwRX5aC5IvhSBksPjBwnEwmrEZ6jTWbJmO7ZtV1OSjqz0yv1OHKD0FJQeXpwM6H00dFGvykDVeQkjdcMm6Tu/zkBh+Zg==",
            PasswordSalt = "9PP9fEheyvtIkayhIPZ1eAcE…eeS77ujlcJIWiX0MZB1Ew=="
        };

        var expected = new List<User>() { user1, user2, user3 };
        var repository = new Mock<IUsersService>();
        repository.Setup(s => s.GetAsync()).ReturnsAsync(expected);
        var controller = new UsersController(repository.Object);

        var actual = await controller.Get();
        Assert.Equal(expected.Count, actual.Count);
    }

    [Fact]
    public async Task Get_ID_given_nonexistent_user_returns_notFound()
    {
        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.GetAsync(It.IsAny<string>())).ReturnsAsync(default(User));
        var controller = new UsersController(repository.Object);

        var actual = await controller.Get("1");

        Assert.IsType<NotFoundResult>(actual.Result);
    }

    [Fact]
    public async Task Get_ID_given_existing_user_returns_user()
    {
        // Arrange
        var expected = new User
        {
            Id = "1",
            UserName = "test123",
            Password = "SomePassword",
            PasswordSalt = "",
            Email = "test.test@gmail.com",
        };

        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.GetAsync("1")).ReturnsAsync(expected);
        var controller = new UsersController(repository.Object);

        // Act
        var actual = await controller.Get("1");

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    [Fact]
    public async Task Get_Username_given_existing_user_returns_user()
    {
        // Arrange
        var expected = new User
        {
            Id = "1",
            UserName = "test123",
            Password = "SomePassword",
            PasswordSalt = "",
            Email = "test.test@gmail.com",
        };

        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.GetUsernameAsync("test123")).ReturnsAsync(expected);
        var controller = new UsersController(repository.Object);

        // Act
        var actual = await controller.GetUsername("test123");

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    [Fact]
    public async Task Get_Username_given_nonexistent_user_returns_notfound()
    {
        // Arrange
        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.GetUsernameAsync("test123")).ReturnsAsync(default(User));
        var controller = new UsersController(repository.Object);

        // Act
        var actual = await controller.GetUsername("test123");

        // Assert
        Assert.IsType<NotFoundResult>(actual.Result);
    }

    [Fact]
    public async Task Post_given_user_returns_201()
    {
        // Arrange
        var user = new User
        {
            Id = "1",
            UserName = "test123",
            Password = "SomePassword",
            Email = "test.test@gmail.com",
        };
        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.CreateAsync(user)).ReturnsAsync(Status.Created);
        var controller = new UsersController(repository.Object);

        // Act
        var actual = await controller.Post(user);

        // Assert
        Assert.IsType<CreatedAtActionResult>(actual);
    }

    [Fact]
    public async Task Post_given_user_returns_400()
    {
        // Arrange
        var user = new User
        {
            Id = "1",
            UserName = "test123",
            Password = "SomePassword",
            Email = "test.test@gmail.com",
        };
        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.CreateAsync(user)).ReturnsAsync(Status.BadRequest);
        var controller = new UsersController(repository.Object);

        // Act
        var actual = await controller.Post(user);

        // Assert
        Assert.IsType<BadRequestResult>(actual);
    }

    [Fact]
    public async Task Update_given_valid_input_returns_204()
    {
        // Arrange
        var expected = new User
        {
            Id = "1",
            UserName = "test123",
            Password = "SomePassword",
            Email = "test.test@gmail.com",
        };
        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.GetAsync("1")).ReturnsAsync(expected);
        var controller = new UsersController(repository.Object);

        // Act
        var actual = await controller.Update("1", expected);

        // Assert
        Assert.IsType<NoContentResult>(actual);
    }

    [Fact]
    public async Task Update_given_invalid_input_returns_404()
    {
        // Arrange
        var expected = new User
        {
            Id = "1",
            UserName = "test123",
            Password = "SomePassword",
            Email = "test.test@gmail.com",
        };
        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.GetAsync("1")).ReturnsAsync(default(User));
        var controller = new UsersController(repository.Object);

        // Act
        var actual = await controller.Update("1", expected);

        // Assert
        Assert.IsType<NotFoundResult>(actual);
    }

    [Fact]
    public async Task Delete_given_valid_input_returns_204()
    {
        // Arrange
        var expected = new User
        {
            Id = "1",
            UserName = "test123",
            Password = "SomePassword",
            Email = "test.test@gmail.com",
        };
        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.GetAsync("1")).ReturnsAsync(expected);
        var controller = new UsersController(repository.Object);

        // Act
        var actual = await controller.Delete("1");

        // Assert
        Assert.IsType<NoContentResult>(actual);
    }

    [Fact]
    public async Task Delete_given_invalid_input_returns_404()
    {
        // Arrange
        var expected = new User
        {
            Id = "1",
            UserName = "test123",
            Password = "SomePassword",
            Email = "test.test@gmail.com",
        };
        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.GetAsync("1")).ReturnsAsync(default(User));
        var controller = new UsersController(repository.Object);

        // Act
        var actual = await controller.Delete("1");

        // Assert
        Assert.IsType<NotFoundResult>(actual);
    }

    [Fact]
    public async Task Signin_given_valid_input_returns_200()
    {
        // Arrange
        var expected = new User
        {
            Id = "1",
            UserName = "lakl",
            Password = "6TmJ+60TCKspLp2lTZyUBZEMjbT53eZmmYrqaD9uwWrW9vXGPdT8qrK1+pmvfrzBDjNuyPZpre+RXhujlPaMYPikTEwyOw==",
            Email = "test.test@gmail.com",
        };
        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.Signin("lakl", "6TmJ+60TCKspLp2lTZyUBZEMjbT53eZmmYrqaD9uwWrW9vXGPdT8qrK1+pmvfrzBDjNuyPZpre+RXhujlPaMYPikTEwyOw==")).ReturnsAsync(expected);
        repository.Setup(s => s.GetSalt("lakl")).ReturnsAsync("Ur8uyPiglLQ0x4Pr9TWQmERH2ciFUDSs8TUdB7S5Uubi13O0NWn0ilgBkTl5BIM3EvHj1EnptYJlUkfS019znmrSX0HKKg==");
        var controller = new UsersController(repository.Object);

        // Act
        var actual = await controller.Signin("lakl", "1234");

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    [Fact]
    public async Task Signin_given_invalid_input_returns_404()
    {
        // Arrange
        var expected = new User
        {
            Id = "1",
            UserName = "lakl",
            Password = "6TmJ+60TCKspLp2lTZyUBZEMjbT53eZmmYrqaD9uwWrW9vXGPdT8qrK1+pmvfrzBDjNuyPZpre+RXhujlPaMYPikTEwyOw==",
            Email = "test.test@gmail.com",
        };
        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.Signin("lakl", "6TmJ+60TCKspLp2lTZyUBZEMjbT53eZmmYrqaD9uwWrW9vXGPdT8qrK1+pmvfrzBDjNuyPZpre+RXhujlPaMYPikTEwyOw==")).ReturnsAsync(default(User));
        repository.Setup(s => s.GetSalt("lakl")).ReturnsAsync("Ur8uyPiglLQ0x4Pr9TWQmERH2ciFUDSs8TUdB7S5Uubi13O0NWn0ilgBkTl5BIM3EvHj1EnptYJlUkfS019znmrSX0HKKg==");
        var controller = new UsersController(repository.Object);

        // Act
        var actual = await controller.Signin("lakl", "1234");

        // Assert
        Assert.IsType<NotFoundResult>(actual.Result);
    } 

    [Fact]
    public async Task Follow_given_valid_input_returns_200()
    {
        // Arrange
        var dto = new FollowDTO()
        {
            WhoID = "1",
            WhomID = "2"
        };
        
        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.Follow("1", "2")).ReturnsAsync(Status.Success);
        var controller = new UsersController(repository.Object);

        // Act
        var actual = await controller.Follow(dto);

        // Assert
        Assert.IsType<OkResult>(actual);
    } 

    [Fact]
    public async Task Follow_given_invalid_input_returns_404()
    {
        // Arrange
        var dto = new FollowDTO()
        {
            WhoID = "1",
            WhomID = "2"
        };

        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.Follow("1", "2")).ReturnsAsync(Status.NotFound);
        var controller = new UsersController(repository.Object);

        // Act
        var actual = await controller.Follow(dto);

        // Assert
        Assert.IsType<NotFoundResult>(actual);
    } 

    [Fact]
    public async Task Follow_given_invalid_input_returns_409()
    {
        // Arrange
        var dto = new FollowDTO()
        {
            WhoID = "1",
            WhomID = "2"
        };

        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.Follow("1", "2")).ReturnsAsync(Status.Conflict);
        var controller = new UsersController(repository.Object);

        // Act
        var actual = await controller.Follow(dto);

        // Assert
        Assert.IsType<ConflictResult>(actual);
    } 

    [Fact]
    public async Task Follow_given_invalid_input_returns_400()
    {
        // Arrange
        var dto = new FollowDTO()
        {
            WhoID = null,
            WhomID = null
        };

        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.Follow("1", "2")).ReturnsAsync(Status.BadRequest);
        var controller = new UsersController(repository.Object);

        // Act
        var actual = await controller.Follow(dto);

        // Assert
        Assert.IsType<BadRequestResult>(actual);
    } 

    [Fact]
    public async Task Unfollow_given_valid_input_returns_200()
    {
        // Arrange
        var dto = new FollowDTO()
        {
            WhoID = "1",
            WhomID = "2"
        };
        
        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.Follow("1", "2")).ReturnsAsync(Status.Success);
        var controller = new UsersController(repository.Object);

        // Act
        var actual = await controller.Follow(dto);

        // Assert
        Assert.IsType<OkResult>(actual);
    } 

    [Fact]
    public async Task Unfollow_given_invalid_input_returns_404()
    {
        // Arrange
        var dto = new FollowDTO()
        {
            WhoID = "1",
            WhomID = "2"
        };

        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.Follow("1", "2")).ReturnsAsync(Status.NotFound);
        var controller = new UsersController(repository.Object);

        // Act
        var actual = await controller.Follow(dto);

        // Assert
        Assert.IsType<NotFoundResult>(actual);
    } 

    [Fact]
    public async Task Unfollow_given_invalid_input_returns_409()
    {
        // Arrange
        var dto = new FollowDTO()
        {
            WhoID = "1",
            WhomID = "2"
        };

        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.Follow("1", "2")).ReturnsAsync(Status.Conflict);
        var controller = new UsersController(repository.Object);

        // Act
        var actual = await controller.Follow(dto);

        // Assert
        Assert.IsType<ConflictResult>(actual);
    } 

    [Fact]
    public async Task Unfollow_given_invalid_input_returns_400()
    {
        // Arrange
        var dto = new FollowDTO()
        {
            WhoID = null,
            WhomID = null
        };

        var repository = new Mock<IUsersService>();

        repository.Setup(s => s.Follow("1", "2")).ReturnsAsync(Status.BadRequest);
        var controller = new UsersController(repository.Object);

        // Act
        var actual = await controller.Follow(dto);

        // Assert
        Assert.IsType<BadRequestResult>(actual);
    } 
}