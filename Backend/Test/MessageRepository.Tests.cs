namespace Test;

public class MessageRepositoryTests
{
    MinitwitContext _context;
    MessageRepository _repo;

    User _user1;
    User _user2;
    User _user3;

    Message _message1;
    Message _message2;

    public MessageRepositoryTests()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var builder = new DbContextOptionsBuilder<MinitwitContext>();
        builder.UseSqlite(connection);
        builder.EnableSensitiveDataLogging();

        var context = new MinitwitContext(builder.Options);
        context.Database.EnsureCreated();

        context.SaveChanges();

        _user1 = new User("user1@use.com", "User1");
        _user2 = new User("user2@use.com", "User2");
        _user3 = new User("user3@use.com", "User3");

        context.users.Add(_user1);
        context.users.Add(_user2);
        context.users.Add(_user3);
        context.SaveChanges();

        _message1 = new Message(_user1.Id, "Hello World!1", 100000L);
        _message2 = new Message(_user2.Id, "Hello World!2", 100000L);

        context.messages.Add(_message1);
        context.messages.Add(_message2);
        context.SaveChanges();

        _context = context;
        _repo = new MessageRepository(_context);
    }

    [Fact]
    public async void Create_Created()
    {
        //Arrange
        var message = new MessageCreateDTO
        {
            AuthorId = _user3.Id,
            Text = "This is text",
            PubDate = 0L
        };

        //Act
        (Response r, MessageDTO messageDTO) = await _repo.CreateAsync(message);
        
        //Assert
        Assert.Equal(Response.Created, r);
        Assert.Equal(message.AuthorId, messageDTO.AuthorId);
        Assert.Equal(message.Text, messageDTO.Text);
        Assert.Equal(message.PubDate, messageDTO.PubDate);

        var entity = await _context.messages.Where(m => m.Id == messageDTO.Id).FirstOrDefaultAsync();
        Assert.NotNull(entity);
        Assert.Equal(messageDTO.Id, entity.Id);
        Assert.Equal(messageDTO.AuthorId, entity.AuthorId);
        Assert.Equal(messageDTO.Text, entity.Text);
        Assert.Equal(messageDTO.PubDate, entity.PubDate);
    }

    [Fact]
    public async void Create_NotFound()
    {
        //Arrange
        var message = new MessageCreateDTO
        {
            AuthorId = 1000,
            Text = "This is text",
            PubDate = 0L
        };

        //Act
        (Response r, MessageDTO messageDTO) = await _repo.CreateAsync(message);
        
        //Assert
        Assert.Equal(Response.NotFound, r);
        Assert.Equal(message.AuthorId, messageDTO.AuthorId);
        Assert.Equal(message.Text, messageDTO.Text);
        Assert.Equal(message.PubDate, messageDTO.PubDate);
    }
    
    
    [Fact]
    public async void ReadAll_returns_all()
    {
        //Act
        var res = await _repo.ReadAllAsync(null, null, null);

        //Assert
        Assert.NotNull(res);

        Assert.Collection<AllMessages>
        (
            res,
            item =>
            {
                Assert.Equal(_user2.Username, item.user);
                Assert.Equal(_message2.PubDate, item.pubDate);
                Assert.Equal(_message2.Text, item.content);
            },

            item =>
            {
                Assert.Equal(_user1.Username, item.user);
                Assert.Equal(_message1.PubDate, item.pubDate);
                Assert.Equal(_message1.Text, item.content);
            }

        );
    }

    [Fact]
    public async void ReadAllByUsername_returns_all()
    {
        
        //Act
        var res = await _repo.ReadAllByUsernameAsync(_user1.Username, null, null);

        //Assert
        Assert.NotNull(res);

        Assert.Collection<MessageDTO>
        (
            res,
            item =>
            {
                Assert.Equal(_user1.Id, item.AuthorId);
                Assert.Equal(_message1.PubDate, item.PubDate);
                Assert.Equal(_message1.Text, item.Text);
            }
        );
    }

    [Fact]
    public async void ClearFollowers_clears_messages()
    {
        //Act
        await _repo.ClearMessages();

        //Assert
        Assert.Empty(_context.messages.AsEnumerable());
    }
}
