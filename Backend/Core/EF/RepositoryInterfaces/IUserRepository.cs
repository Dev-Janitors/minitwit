namespace Backend.Core.EF;

public interface IUserRepository
{
    //Create
    Task<(Response, UserDTO)> CreateAsync(UserCreateDTO user);

    //Delete
    Task<Response> ClearUsers();

    //Update
    Task<Response> UpdateAsync(UserUpdateDTO user);

    //Delete
    Task<Response> RemoveAsync(int id);
    //Read
    Task<Option<UserDTO>> ReadByIDAsync(int userID);

    Task<Option<UserDTO>> ReadByEmailAsync(string Email);

    Task<Option<UserDTO>> ReadByUsernameAsync(string Nickname);
    Task<Response> Seed();

    Task<IReadOnlyCollection<UserDTO>> ReadAllAsync();
    
}