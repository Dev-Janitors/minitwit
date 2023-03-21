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
    Task<IReadOnlyCollection<MessageDTO>> ReadAllByUsernameAsync(string Username, int? startIndex, int? endIndex);
    Task<int> Seed();
    Task<IReadOnlyCollection<AllMessages>> ReadAllAsync(int? n, int? start, int? end);




}