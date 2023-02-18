namespace Backend.Infrastructure;

using System.Threading.Tasks;
using Backend.Core;
using Backend.Core.EF;
using Microsoft.AspNetCore.Http.HttpResults;

public class FollowerRepository : IFollowerRepository
{
  private readonly MinitwitContext _context;

  public FollowerRepository(MinitwitContext context)
  {
    _context = context;
  }

  public async Task<Response> ClearFollowers()
  {
    _context.followers.RemoveRange(_context.followers);
    await _context.SaveChangesAsync();
    return Response.Deleted;
  }

  public async Task<(Response, FollowerDTO)> CreateAsync(FollowerCreateDTO follow)
  {
    var conflict = await _context.followers
      .Where(f => f.WhoId == follow.WhoId && f.WhomId == follow.WhomId)
      .FirstOrDefaultAsync();

    //Return conflict if the follow already exists
    if (conflict != null) return (Response.Conflict, new FollowerDTO(-1, follow.WhoId, follow.WhomId));

    //Check if users exist
    bool whoExists = await _context.users.AnyAsync(u => u.Id == follow.WhoId);
    bool whomExists = await _context.users.AnyAsync(u => u.Id == follow.WhomId);
    if (!whoExists || !whomExists) return (Response.NotFound, new FollowerDTO(-1, follow.WhoId, follow.WhomId));

    var entity = new Follower 
    (
      follow.WhoId,
      follow.WhomId
    );

    await _context.followers.AddAsync(entity);
    await _context.SaveChangesAsync();

    return (Response.Created, new FollowerDTO(entity.Id, entity.WhoId, entity.WhomId));
  }

  public async Task<Response> DeleteByIdAsync(int id)
  {
    Follower? follow = await (
      _context.followers
        .Where(f => f.Id == id)
        .FirstOrDefaultAsync()
    );
    
    if (follow == null) return Response.NotFound;

    _context.Remove(follow);
    await _context.SaveChangesAsync();

    return Response.Deleted;
  }

  public async Task<Response> DeleteByWhoAndWhomIds(int whoId, int whomId)
  {
    Follower? follow = await (
      _context.followers
        .Where(f => f.WhoId == whoId && f.WhomId == whomId)
        .FirstOrDefaultAsync()
    );
    
    if (follow == null) return Response.NotFound;

    _context.Remove(follow);
    await _context.SaveChangesAsync();

    return Response.Deleted;
  }

  public Task<Option<FollowerDTO>> ReadById(int id)
  {
    throw new NotImplementedException();
  }

  public async Task<Option<FollowerDTO>> ReadByWhoAndWhomId(int whoId, int whomId)
  {
    var follow = await _context.followers
      .Where(f => f.WhoId == whoId && f.WhomId == whomId)
      .Select(f => new FollowerDTO(
          f.Id,
          f.WhoId,
          f.WhomId
        )
      )
      .FirstAsync();
    
    return follow;
  }

  public Task<Response> UpdateAsync(FollowerUpdateDTO follower)
  {
    throw new NotImplementedException();
  }

  public async Task<IReadOnlyCollection<FollowerDTO>> ReadAllByWhomId(int whomId)
  {
    var followers = _context.followers
      .Where(f => f.WhomId == whomId)
      .Select(f => new FollowerDTO(
          f.Id,
          f.WhoId,
          f.WhomId
        )
      );
    return await followers.ToListAsync();
  }
}