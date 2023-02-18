namespace Backend.Core.EF;

public interface IMessageRepository {

    //Create
    Task<(Response, MessageDTO)> CreateAsync(MessageCreateDTO user);

    //Delete
    Task<Response> ClearMessages();

    //Update
    Task<Response> UpdateAsync(MessageUpdateDTO user);

    //Delete
    Task<Response> RemoveAsync(int id);
    //Read
    Task<IReadOnlyCollection<MessageDTO>> ReadAllByAuthorIDAsync(int userID);

    Task<IReadOnlyCollection<MessageDTO>> ReadAllByUsernameAsync(string Nickname);

    Task<Response> Seed();

    Task<IReadOnlyCollection<MessageDTO>> ReadAllAsync();




}