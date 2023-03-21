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

    public async Task<Response> ClearMessages()
    {
        await _context.messages.ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
        return Response.Deleted;
    }

    public async Task<int> Seed()
    {
        //create a list of messages
        var messages = new List<Message>(){
          new Message(1, "Hello World", 1676552060),
          new Message(2, "Hi", 1676552063),
          new Message(1, "Perle", 1676552068),
          new Message(2, "Circus", 1676552070),
          new Message(1, "Bordtennis", 1676552080),
          new Message(2, "Vi skal på grillen", 1676552090),
          new Message(1, "Fodbold", 1676552100),
          new Message(2, "Ingenting", 1676552130),
          new Message(1, "John", 1676552180),
        };

        foreach (var message in messages)
        {
            await _context.messages.AddAsync(message);
        }

        var writtenChanges = await _context.SaveChangesAsync();

        return writtenChanges;
    }

    public async Task<(Response, MessageDTO)> CreateAsync(MessageCreateDTO message)
    {
        if (!_context.users.Any(u => u.Id == message.AuthorId)) return (Response.NotFound, new MessageDTO(-1, message.AuthorId, message.Text ?? "", message.PubDate, 0));
        Message entity = new Message(message.AuthorId, message.Text!, message.PubDate);
        _context.messages.Add(entity);
        await _context.SaveChangesAsync();
        return (Response.Created, new MessageDTO(entity.Id, entity.AuthorId, entity.Text, entity.PubDate, entity.Flagged));
    }

    public async Task<IReadOnlyCollection<AllMessages>> ReadAllAsync(int? start, int? end)
    {
        var query = from message in _context.messages
                    join user in _context.users on message.AuthorId equals user.Id
                    orderby message.Id descending
                    select new AllMessages
                    {
                        content = message.Text,
                        user = user.Username,
                        pubDate = message.PubDate
                    };

        if (start != null) query = query.Skip(start.Value);
        if (end != null && start != null) query = query.Take(end.Value - start.Value);
        if (end != null) query = query.Take(end.Value);

        return await query.ToListAsync();
    }

    public Task<IReadOnlyCollection<MessageDTO>> ReadAllByAuthorIDAsync(int userID)
    {
        // var messages = from m in _context.messages
        //                   where m.AuthorId == userID
        //                   select m.ToDto()
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<AllMessages>> ReadAllByAuthorIDListAsync(IEnumerable<int> authorIDList, int? startIndex, int? endIndex)
    {
        var messages = await _context.messages
          .Join(_context.users, m => m.AuthorId, u => u.Id, (m, u) => new
          {
              authorId = u.Id,
              username = u.Username,
              messageId = m.Id,
              content = m.Text,
              pubDate = m.PubDate
          })
          .Where(m => authorIDList.Contains(m.authorId))
          .OrderByDescending(m => m.messageId)
          .Select(m => new AllMessages
          {
              content = m.content,
              user = m.username,
              pubDate = m.pubDate
          })
          .ToListAsync();

        if (startIndex != null) messages = messages.Skip(startIndex.Value).ToList();
        if (endIndex != null && startIndex != null) messages = messages.Take(endIndex.Value - startIndex.Value).ToList();
        if (endIndex != null) messages = messages.Take(endIndex.Value).ToList();

        return messages;
    }

    public async Task<IReadOnlyCollection<MessageDTO>> ReadAllByUsernameAsync(string username, int? startIndex, int? endIndex)
    {
        var authorId = await _context.users
          .Where(u => u.Username == username)
          .Select(u => u.Id)
          .FirstOrDefaultAsync();

        var messages = await _context.messages
          .Where(m => m.AuthorId == authorId)
          .OrderByDescending(m => m.Id)
          .Select(m => new MessageDTO(m.Id, m.AuthorId, m.Text, m.PubDate, m.Flagged))
          .ToListAsync();

        if (startIndex != null) messages = messages.Skip(startIndex.Value).ToList();
        if (endIndex != null && startIndex != null) messages = messages.Take(endIndex.Value - startIndex.Value).ToList();
        if (endIndex != null) messages = messages.Take(endIndex.Value).ToList();

        return messages;
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