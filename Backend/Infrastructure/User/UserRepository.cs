namespace Backend.Infrastructure;

using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Core;
using Backend.Core.EF;

public class UserRepository : IUserRepository
{
private readonly MinitwitContext _dbcontext;

public UserRepository(MinitwitContext context)
{
    _dbcontext = context;
}

  public async Task<Response> ClearUsers(){
    _dbcontext.users.RemoveRange(_dbcontext.users);
    await _dbcontext.SaveChangesAsync();
    return Response.Deleted;
  }

  public async Task<Response> Seed()
  {
    //create a list of messages
    var users = new List<User>(){
      new User("test@test.com", "robot"),
      new User("bob@mail.com", "bob")
    };
    
    foreach (var user in users)
    {
        await _dbcontext.users.AddAsync(user);
    }
    await _dbcontext.SaveChangesAsync();

    return Response.Created;
  }
  public async Task<(Response, UserDTO)> CreateAsync(UserCreateDTO user)
  {
    {
    var conflict = await _dbcontext.users
        .Where(u => u.Email == user.Email || (u.Username != null && u.Username == user.Username))
        .Select(u => new UserDTO(u.Id, u.Email, u.Username))
        .FirstOrDefaultAsync();
    
    if (conflict != null)
    {
        return (Response.Conflict, conflict);
    }
    

    var entity = new User(user.Email,user.Username);

    _dbcontext.users.Add(entity);

    await _dbcontext.SaveChangesAsync();

    return (Response.Created, new UserDTO(entity.Id, entity.Email, entity.Username));
}

  }

  public async Task<IReadOnlyCollection<UserDTO>> ReadAllAsync()
  {
    return(await _dbcontext.users
                        .Select(u => new UserDTO(u.Id, u.Email, u.Username))
                        .ToListAsync())
                        .AsReadOnly();
    
  }

  public async Task<Option<UserDTO>> ReadByEmailAsync(string Email)
  {
    var users = from u in _dbcontext.users
                        where u.Email == Email
                        select new UserDTO(u.Id, u.Email, u.Username);
        
        return await users.FirstOrDefaultAsync();
  }

  public async Task<Option<UserDTO>> ReadByIDAsync(int userID)
  {
     var users = from u in _dbcontext.users
                        where u.Id == userID
                        select new UserDTO(u.Id, u.Email, u.Username);

        return await users.FirstOrDefaultAsync();
  }

  public async Task<Option<UserDTO>> ReadByUsernameAsync(string Username)
  {
    var users = from u in _dbcontext.users
                        where u.Username == Username
                        select new UserDTO(u.Id, u.Email, u.Username);
                        
        return await users.FirstOrDefaultAsync();
  }

  public Task<Response> RemoveAsync(int id)
  {
    throw new NotImplementedException();
  }

  public Task<Response> UpdateAsync(UserUpdateDTO user)
  {
    throw new NotImplementedException();
  }
}