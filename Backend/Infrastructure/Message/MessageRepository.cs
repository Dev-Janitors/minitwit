namespace Backend.Infrastructure;

using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Core;
using Backend.Core.EF;

public class MessageRepository : IMessageRepository
{
    private readonly MinitwitContext _context;

    public MessageRepository(MinitwitContext context)
    {
        _context = context;
    }

    public async Task<Response> Seed()
    {
        //create a list of messages
        var messages = new List<Message>(){
          new Message(1, "Hello World", 12903871),
          new Message(1, "Hi", 12903873),
          new Message(1, "Perle", 12903875),
          new Message(1, "Circus", 12903877),
          new Message(1, "Bordtennis", 12903879),
          new Message(1, "Vi skal på grillen", 12903881),
          new Message(1, "Fodbold", 12903883),
          new Message(1, "Ingenting", 12903885),
          new Message(1, "John", 12903887),
        };
        
        foreach (var message in messages)
        {
            await _context.messages.AddAsync(message);
        }
        await _context.SaveChangesAsync();

        return Response.Created;
    }

  public async Task<(Response, MessageDTO)> CreateAsync(MessageCreateDTO message)
  {
    var entity = new Message(message.AuthorId, message.Text, message.PubDate);
    _context.messages.Add(entity);
    await _context.SaveChangesAsync();
    return (Response.Created, new MessageDTO(entity.Id, entity.AuthorId, entity.Text, entity.PubDate, entity.Flagged));
  }

  public async Task<IReadOnlyCollection<MessageDTO>> ReadAllAsync()
  {
    var messages = await _context.messages.Select(
      m => new MessageDTO(
        m.Id,
        m.AuthorId,
        m.Text,
        m.PubDate,
        m.Flagged
      )
    ).ToListAsync();
    return messages.AsReadOnly();
  }

  public Task<Option<MessageDTO>> ReadByAsync(string Email)
  {
    throw new NotImplementedException();
  }

  public Task<Option<MessageDTO>> ReadByAuthorIDAsync(int userID)
  {
    throw new NotImplementedException();
  }

  public Task<Option<MessageDTO>> ReadByUsernameAsync(string Nickname)
  {
    throw new NotImplementedException();
  }

  public Task<Response> RemoveAsync(int id)
  {
    throw new NotImplementedException();
  }

  public Task<Response> UpdateAsync(MessageUpdateDTO user)
  {
    throw new NotImplementedException();
  }
}