namespace Test;

public class FollowerRepositoryTests
{
    MinitwitContext _context;
    FollowerRepository _repo;

    User _user1;
    User _user2;
    User _user3;

    public FollowerRepositoryTests()
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

        _context = context;
        _repo = new FollowerRepository(_context);
    }


    [Fact]
    public async void ClearFollowers_clears_followers()
    {
        //Arrange
        var follow1 = new Follower(_user1.Id, _user2.Id);
        var follow2 = new Follower(_user2.Id, _user1.Id);
        _context.followers.Add(follow1);
        _context.followers.Add(follow2);
        await _context.SaveChangesAsync();

        //Act
        await _repo.ClearFollowers();

        //Assert
        Assert.Empty(_context.followers.AsEnumerable());
    }

    [Fact]
    public async void CreateAsync_Created()
    {
        //Arrange
        var follow = new FollowerCreateDTO
        {
            WhoId = _user1.Id,
            WhomId = _user2.Id
        };

        //Act
        (Response r, FollowerDTO followerDTO) = await _repo.CreateAsync(follow);

        //Assert
        Assert.Equal(Response.Created, r);
        Assert.Equal(_user1.Id ,followerDTO.WhoId);
        Assert.Equal(_user2.Id ,followerDTO.WhomId);

        var entity = await _context.followers.Where(f => f.Id == followerDTO.Id).FirstOrDefaultAsync();
        Assert.NotNull(entity);
        Assert.Equal(_user1.Id ,entity.WhoId);
        Assert.Equal(_user2.Id ,entity.WhomId);
    }

    [Fact]
    public async void CreateAsync_Conflict()
    {
        //Arrange
        var follow = new FollowerCreateDTO
        {
            WhoId = _user1.Id,
            WhomId = _user2.Id
        };

        //Act
        await _repo.CreateAsync(follow);
        (Response r, FollowerDTO followerDTO) = await _repo.CreateAsync(follow);

        //Assert
        Assert.Equal(Response.Conflict, r);
        Assert.Equal(_user1.Id ,followerDTO.WhoId);
        Assert.Equal(_user2.Id ,followerDTO.WhomId);
    }

    [Fact]
    public async void CreateAsync_NotFound()
    {
        //Arrange
        var follow = new FollowerCreateDTO
        {
            WhoId = 1000,
            WhomId = _user2.Id
        };

        //Act
        (Response r, FollowerDTO followerDTO) = await _repo.CreateAsync(follow);

        //Assert
        Assert.Equal(Response.NotFound, r);
        Assert.Equal(1000 ,followerDTO.WhoId);
        Assert.Equal(_user2.Id ,followerDTO.WhomId);
    }

    [Fact]
    public async void ReadAllBy_returns_all()
    {
        //Arrange
        var follow1 = new FollowerCreateDTO
        {
            WhoId = _user1.Id,
            WhomId = _user2.Id
        };

         var follow2 = new FollowerCreateDTO
        {
            WhoId = _user2.Id,
            WhomId = _user1.Id
        };

        //Act
        await _repo.CreateAsync(follow1);
        await _repo.CreateAsync(follow2);

        var list = await _repo.ReadAll();

        //Assert
        Assert.NotNull(list);
        Assert.Collection<FollowerDTO>
        (
            list,
            item =>
            {
                Assert.Equal(1, item.Id);
                Assert.Equal(follow1.WhoId, item.WhoId);
                Assert.Equal(follow1.WhomId, item.WhomId);
                
            },

            item =>
            {
                Assert.Equal(2, item.Id);
                Assert.Equal(follow2.WhoId, item.WhoId);
                Assert.Equal(follow2.WhomId, item.WhomId);
                
            }
        );
    }

    
    
    [Fact]
    public async void ReadByWhoAndWhomId_Reads()
    {
        //Arrange
        var follow1 = new Follower(_user1.Id, _user2.Id);
        var follow2 = new Follower(_user2.Id, _user1.Id);
        _context.followers.Add(follow1);
        _context.followers.Add(follow2);
        await _context.SaveChangesAsync();

        //Act
        var follow = await _repo.ReadByWhoAndWhomId(_user1.Id, _user2.Id);

        //Assert
        Assert.True(follow.IsSome);
        Assert.Equal(_user1.Id, follow.Value.WhoId);
        Assert.Equal(_user2.Id, follow.Value.WhomId);
    }
    
    [Fact]
    public async void ReadByWhoAndWhomId_None()
    {
        //Act
        var follow = await _repo.ReadByWhoAndWhomId(1, 2);

        //Assert
        Assert.True(follow.IsNone);
    }
}