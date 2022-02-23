using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;

namespace Server.Tests;

public class MessagesControllerTests
{
    // If you're looking for the previous constructor Caspar, its under notion. I did not see the reason for it to be here.

    [Fact]
    public async Task Get_returns_messages_from_collectionAsync()
    {
        // Arrange
        var expected = new List<Message>();
        var repository = new Mock<IMessagesService>();
        repository.Setup(s => s.GetAsync()).ReturnsAsync(expected);
        var controller = new MessagesController(repository.Object);

        // Act
        var actual = await controller.Get();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Get_given_message_id_returns_200()
    {
        // Arrange
        var expected = new Message
        {
            Id = "1",
            AuthorID = "1",
            AuthorName = "Frederik",
            Timestamp = new System.DateTime(2022, 1, 1),
            Text = "Hi there, this is a test."
        };

        var repository = new Mock<IMessagesService>();
        repository.Setup(s => s.GetAsync("1")).ReturnsAsync(expected);
        var controller = new MessagesController(repository.Object);

        // Act
        var actual = await controller.Get("1");

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    [Fact]
    public async Task Get_given_invalid_id_returns_404()
    {
        // Arrange
        var expected = new Message
        {
            Id = "1",
            AuthorID = "1",
            AuthorName = "Frederik",
            Timestamp = new System.DateTime(2022, 1, 1),
            Text = "Hi there, this is a test."
        };

        var repository = new Mock<IMessagesService>();
        repository.Setup(s => s.GetAsync("1")).ReturnsAsync(default(Message));
        var controller = new MessagesController(repository.Object);

        // Act
        var actual = await controller.Get("1");

        // Assert
        Assert.IsType<NotFoundResult>(actual.Result);
    }

    [Fact]
    public async Task GetFollowingMessages_given_userID_from_followers_returns_200()
    {
        // Arrange
        var msg1 = new Message
        {
            Id = "1",
            AuthorID = "1",
            AuthorName = "Rocky",
            Timestamp = new System.DateTime(2022, 1, 1),
            Text = "Hi there, this is a test."
        };
        var msg2 = new Message
        {
            Id = "2",
            AuthorID = "1",
            AuthorName = "Rocky",
            Timestamp = new System.DateTime(2022, 1, 1),
            Text = "Hi there, this is a test."
        };

        var expected = new List<Message>() { msg1, msg2 };
        var repository = new Mock<IMessagesService>();
        repository.Setup(s => s.GetMessagesFromFollowing("2")).ReturnsAsync(expected);
        var controller = new MessagesController(repository.Object);

        // Act
        var actual = await controller.GetFollowingMessages("2");

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFollowingMessages_given_invalid_input_returns_404()
    {
        // Arrange
        var msg1 = new Message
        {
            Id = "1",
            AuthorID = "1",
            AuthorName = "Rocky",
            Timestamp = new System.DateTime(2022, 1, 1),
            Text = "Hi there, this is a test."
        };
        var msg2 = new Message
        {
            Id = "2",
            AuthorID = "1",
            AuthorName = "Rocky",
            Timestamp = new System.DateTime(2022, 1, 1),
            Text = "Hi there, this is a test."
        };

        var expected = new List<Message>() { msg1, msg2 };
        var repository = new Mock<IMessagesService>();
        repository.Setup(s => s.GetMessagesFromFollowing("2")).ReturnsAsync(default(List<Message>));
        var controller = new MessagesController(repository.Object);

        // Act
        var actual = await controller.GetFollowingMessages("2");

        // Assert
        Assert.IsType<NotFoundResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromUserID_given_valid_input_returns_200()
    {
        // Arrange
        var msg1 = new Message
        {
            Id = "1",
            AuthorID = "1",
            AuthorName = "Rocky",
            Timestamp = new System.DateTime(2022, 1, 1),
            Text = "Hi there, this is a test."
        };
        var msg2 = new Message
        {
            Id = "2",
            AuthorID = "1",
            AuthorName = "Rocky",
            Timestamp = new System.DateTime(2022, 1, 1),
            Text = "Hi there, this is a test."
        };


        var expected = new List<Message>() { msg1, msg2 };
        var repository = new Mock<IMessagesService>();
        repository.Setup(s => s.GetMessageFromUserIDAsync("2")).ReturnsAsync(expected);
        var controller = new MessagesController(repository.Object);

        // Act
        var actual = await controller.GetFromUserID("2");

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromUserID_given_invalid_input_returns_404()
    {
        // Arrange
        var msg1 = new Message
        {
            Id = "1",
            AuthorID = "1",
            AuthorName = "Rocky",
            Timestamp = new System.DateTime(2022, 1, 1),
            Text = "Hi there, this is a test."
        };
        var msg2 = new Message
        {
            Id = "2",
            AuthorID = "1",
            AuthorName = "Rocky",
            Timestamp = new System.DateTime(2022, 1, 1),
            Text = "Hi there, this is a test."
        };


        var expected = new List<Message>() { msg1, msg2 };
        var repository = new Mock<IMessagesService>();
        repository.Setup(s => s.GetMessageFromUserIDAsync("2")).ReturnsAsync(default(List<Message>));
        var controller = new MessagesController(repository.Object);

        // Act
        var actual = await controller.GetFromUserID("2");

        // Assert
        Assert.IsType<NotFoundResult>(actual.Result);
    }

    [Fact]
    public async Task Post_given_valid_input_returns_201Created_response()
    {
        // Arrange
        var expected = new Message
        {
            Id = "1",
            AuthorID = "1",
            AuthorName = "Rocky",
            Timestamp = new System.DateTime(2022, 1, 1),
            Text = "Hi there, this is a test."
        };

        var repository = new Mock<IMessagesService>();
        repository.Setup(s => s.CreateAsync(expected)).ReturnsAsync(Status.Created);
        var controller = new MessagesController(repository.Object);

        // Act
        var actual = await controller.Post(expected);

        // Assert
        Assert.IsType<CreatedAtActionResult>(actual);
    }

    [Fact]
    public async Task Post_given_invalid_input_returns_400_response()
    {
        // Arrange
        var expected = new Message
        {
            Id = "1",
            AuthorID = "1",
            AuthorName = "Rocky",
            Timestamp = new System.DateTime(2022, 1, 1),
            Text = "Hi there, this is a test."
        };

        var repository = new Mock<IMessagesService>();
        repository.Setup(s => s.CreateAsync(expected)).ReturnsAsync(Status.BadRequest);
        var controller = new MessagesController(repository.Object);

        // Act
        var actual = await controller.Post(expected);

        // Assert
        Assert.IsType<BadRequestResult>(actual);
    }

    [Fact]
    public async Task Update_given_valid_input_returns_204_response()
    {
        // Arrange
        var expected = new Message
        {
            Id = "1",
            AuthorID = "1",
            AuthorName = "Rocky",
            Timestamp = new System.DateTime(2022, 1, 1),
            Text = "Hi there, this is a test."
        };

        var repository = new Mock<IMessagesService>();
        repository.Setup(s => s.GetAsync("1")).ReturnsAsync(expected);
        var controller = new MessagesController(repository.Object);

        // Act
        var actual = await controller.Update("1", expected);

        // Assert
        Assert.IsType<NoContentResult>(actual);
    }

    [Fact]
    public async Task Update_given_invalid_input_returns_404_response()
    {
        // Arrange
        var expected = new Message
        {
            Id = "1",
            AuthorID = "1",
            AuthorName = "Rocky",
            Timestamp = new System.DateTime(2022, 1, 1),
            Text = "Hi there, this is a test."
        };

        var repository = new Mock<IMessagesService>();
        repository.Setup(s => s.GetAsync("1")).ReturnsAsync(default(Message));
        var controller = new MessagesController(repository.Object);

        // Act
        var actual = await controller.Update("1", expected);

        // Assert
        Assert.IsType<NotFoundResult>(actual);
    }

    [Fact]
    public async Task Delete_given_valid_input_returns_204_response()
    {
        // Arrange
        var expected = new Message
        {
            Id = "1",
            AuthorID = "1",
            AuthorName = "Rocky",
            Timestamp = new System.DateTime(2022, 1, 1),
            Text = "Hi there, this is a test."
        };

        var repository = new Mock<IMessagesService>();
        repository.Setup(s => s.GetAsync("1")).ReturnsAsync(expected);
        var controller = new MessagesController(repository.Object);

        // Act
        var actual = await controller.Delete("1");

        // Assert
        Assert.IsType<NoContentResult>(actual);
    }

    [Fact]
    public async Task Delete_given_invalid_input_returns_404_response()
    {
        // Arrange
        var expected = new Message
        {
            Id = "1",
            AuthorID = "1",
            AuthorName = "Rocky",
            Timestamp = new System.DateTime(2022, 1, 1),
            Text = "Hi there, this is a test."
        };

        var repository = new Mock<IMessagesService>();
        repository.Setup(s => s.GetAsync("1")).ReturnsAsync(default(Message));
        var controller = new MessagesController(repository.Object);

        // Act
        var actual = await controller.Delete("1");

        // Assert
        Assert.IsType<NotFoundResult>(actual);
    }
}