namespace Test;
public class UserRepositoryTests
{
    MinitwitContext _context;

    UserRepository _repo;
    //Setup
    public UserRepositoryTests()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var builder = new DbContextOptionsBuilder<MinitwitContext>();
        builder.UseSqlite(connection);
        builder.EnableSensitiveDataLogging();

        var context = new MinitwitContext(builder.Options);
        context.Database.EnsureCreated();

        _context = context;
        _repo = new UserRepository(_context);
    }


    //Create

    [Fact]
    public async void CreateAsync_Created()
    {
        //Arrange
        var userCreateDTO = new UserCreateDTO{
            Email = "jhon@jhonson.com",
            Username = "Johnny"
        };

        //Act
        (Response response, UserDTO userDTO) = await _repo.CreateAsync(userCreateDTO);

        var entity = await _context.users
            .Where(u => u.Email == userCreateDTO.Email)
            .FirstAsync();

        //Assert

        Assert.NotNull(entity);
        Assert.Equal(1, entity.Id);
        Assert.Equal(userCreateDTO.Email, entity.Email);
        Assert.Equal(userCreateDTO.Username, entity.Username);

        Assert.Equal(Response.Created, response);
        Assert.Equal(entity.Id, userDTO.Id);
        Assert.Equal(userCreateDTO.Email, userDTO.Email);
        Assert.Equal(userCreateDTO.Username, userDTO.Username);
    }

    [Fact]
    public async void CreateAsync_User_with_same_email_Conflict()
    {
        //Arrange
        var userCreateDTO = new UserCreateDTO{
            Email = "jhon@jhonson.com",
            Username = "Johnny"
        };

        //Act
        await _repo.CreateAsync(userCreateDTO);
        (Response response, UserDTO userDTO) = await _repo.CreateAsync(userCreateDTO);

        //Assert
        Assert.Equal(Response.Conflict, response);
        Assert.Equal(userCreateDTO.Email, userDTO.Email);
        Assert.Equal(userCreateDTO.Username, userDTO.Username);
    }


    //ReadById
    [Fact]
    public async void ReadById_returns_UserDTO()
    {
        //Arrange
        var userCreateDTO = new UserCreateDTO{
            Email = "jhon@jhonson.com",
            Username = "Johnny"
        };

        //Act
        await _repo.CreateAsync(userCreateDTO);
        await _context.SaveChangesAsync();
        
        var result = await _repo.ReadByIDAsync(1);
        

        //Assert
        Assert.True(result.IsSome);
        Assert.Equal(1, result.Value.Id);
        Assert.Equal(userCreateDTO.Email, result.Value.Email);
        Assert.Equal(userCreateDTO.Username, result.Value.Username);
    }

    //ReadByEmail
    [Fact]
    public async void ReadByEmail_returns_UserDTO()
    {
        //Arrange
        var userCreateDTO = new UserCreateDTO{
            Email = "jhon@jhonson.com",
            Username = "Johnny"
        };

        //Act
        await _repo.CreateAsync(userCreateDTO);
        await _context.SaveChangesAsync();
        
        var result = await _repo.ReadByEmailAsync(userCreateDTO.Email);
        

        //Assert
        Assert.True(result.IsSome);
        Assert.Equal(1, result.Value.Id);
        Assert.Equal(userCreateDTO.Email, result.Value.Email);
        Assert.Equal(userCreateDTO.Username, result.Value.Username);
    }

    //Read by Username

    [Fact]
    public async void ReadByUsername_returns_UserDTO()
    {
        //Arrange
        var userCreateDTO = new UserCreateDTO{
            Email = "jhon@jhonson.com",
            Username = "Johnny"
        };

        

        //Act
        await _repo.CreateAsync(userCreateDTO);
        await _context.SaveChangesAsync();
        
        var result = await _repo.ReadByUsernameAsync(userCreateDTO.Username);
        

        //Assert
        Assert.True(result.IsSome);
        Assert.Equal(1, result.Value.Id);
        Assert.Equal(userCreateDTO.Email, result.Value.Email);
        Assert.Equal(userCreateDTO.Username, result.Value.Username);
    }

    

    [Fact]
    public async void ReadById_returns_null()
    {
        //Act
        var result = await _repo.ReadByIDAsync(1);
        
        //Assert
        Assert.True(result.IsNone);
    }

    [Fact]
    public async void ReadAll_returns_all()
    {
        //Arrange
         var userCreateDTO1 = new UserCreateDTO{
            Email = "jhon@jhonson.com",
            Username = "Johnny"
        
        };

        var userCreateDTO2 = new UserUpdateDTO
        {
            Email = "pablo@pabloson.com",
            Username = "Pablo"
        };

        //Act

        await _repo.CreateAsync(userCreateDTO1);
        await _repo.CreateAsync(userCreateDTO2);

        var res = await _repo.ReadAllAsync();

        //Assert
        Assert.NotNull(res);
        
        Assert.Collection<UserDTO>
        (
            res,
            item =>
            {
                Assert.Equal(1, item.Id);
                Assert.Equal(userCreateDTO1.Email, item.Email);
                Assert.Equal(userCreateDTO1.Username, item.Username);

            },

            item =>
            {
                Assert.Equal(2, item.Id);
                Assert.Equal(userCreateDTO2.Email, item.Email);
                Assert.Equal(userCreateDTO2.Username, item.Username);
                
            }
        );
    }


       [Fact] 
       public async void ReadAll_returns_null(){
        
        //Act
        var result = await _repo.ReadAllAsync();

        //Assert
        Assert.Empty(result.AsEnumerable());

       }
}
