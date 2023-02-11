namespace Backend.Core.EF;

public interface IMessageRepository {

    //Create
    Task<(Response, MessageDTO)> CreateAsync(MessageCreateDTO user);

    //Update
    Task<Response> UpdateAsync(MessageUpdateDTO user);

    //Delete
    Task<Response> RemoveAsync(int id);
    //Read
    Task<Option<MessageDTO>> ReadByAuthorIDAsync(int userID);

    Task<Option<MessageDTO>> ReadByAsync(string Email);

    Task<Option<MessageDTO>> ReadByUsernameAsync(string Nickname);

    Task<Response> Seed();

    Task<IReadOnlyCollection<MessageDTO>> ReadAllAsync();




}