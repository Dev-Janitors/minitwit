namespace Backend.Infrastructure;

using System.Threading.Tasks;
using Backend.Core;
using Backend.Core.EF;

public class FollowerRepository : IFollowerRepository
{
    private readonly MinitwitContext _context;

    public FollowerRepository(MinitwitContext context)
    {
      _context = context;
    }

  Task<(Response, FollowerDTO)> IFollowerRepository.CreateAsync(FollowerCreateDTO user)
  {
    throw new NotImplementedException();
  }

  Task<Response> IFollowerRepository.DeleteByIdAsync(int id)
  {
    throw new NotImplementedException();
  }

  Task<Response> IFollowerRepository.DeleteByWhoAndWhomIds(int whoId, int whomId)
  {
    throw new NotImplementedException();
  }

  Task<Option<FollowerDTO>> IFollowerRepository.ReadById(int id)
  {
    throw new NotImplementedException();
  }

  Task<Option<FollowerDTO>> IFollowerRepository.ReadByWhoAndWhomId(int whoId, int whomId)
  {
    throw new NotImplementedException();
  }

  Task<Response> IFollowerRepository.UpdateAsync(FollowerUpdateDTO follower)
  {
    throw new NotImplementedException();
  }

  public async Task<IReadOnlyCollection<FollowerDTO>>ReadAllByWhomId(int whomId)
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