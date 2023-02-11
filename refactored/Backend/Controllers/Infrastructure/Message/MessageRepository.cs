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
          new Message(4040404, "Hello World", 12903871),
          new Message(4040405, "Hi", 12903873),
          new Message(4040406, "Perle", 12903875),
          new Message(4040407, "Circus", 12903877),
          new Message(4040408, "Bordtennis", 12903879),
          new Message(4040409, "Vi skal på grillen", 12903881),
          new Message(4040410, "Fodbold", 12903883),
          new Message(4040411, "Ingenting", 12903885),
          new Message(4040412, "John", 12903887),
        };
        
        foreach (var message in messages)
        {
            await _context.messages.AddAsync(message);
        }
        await _context.SaveChangesAsync();

        return Response.Created;
    }

  public Task<(Response, MessageDTO)> CreateAsync(MessageCreateDTO user)
  {
    throw new NotImplementedException();
  }

  public async Task<IReadOnlyCollection<MessageDTO>> ReadAllAsync()
  {
    var messages = await _context.messages.Select(
        m => new MessageDTO(
            m.Id,
            m.MessageId,
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