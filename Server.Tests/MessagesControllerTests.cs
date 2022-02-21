using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;

namespace Server.Tests;

public class MessagesControllerTests
{    
    // If you're looking for the previous constructor Caspar, its under notion. I did not see the reason for it to be here.

    [Fact]
    public async Task Get_returns_messages_from_collectionAsync()
    {
        var expected = new List<Message>();
        var repository = new Mock<IMessagesService>();
        repository.Setup(s => s.GetAsync()).ReturnsAsync(expected);
        var controller = new MessagesController(repository.Object);

        var actual = await controller.Get(); 
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Get_given_message_id_returns_message()
    {
        var expected = new Message
        {
            Id = "1",
            AuthorID = "1",
            AuthorName = "Frederik",
            Timestamp = new System.DateTime(2022,1,1),
            Text = "Hi there, this is a test."
        };

        var repository = new Mock<IMessagesService>();
        repository.Setup(s => s.GetAsync("1")).ReturnsAsync(expected);
        var controller = new MessagesController(repository.Object);

        var actual = await controller.Get("1");

        Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual.Value));
    }
}