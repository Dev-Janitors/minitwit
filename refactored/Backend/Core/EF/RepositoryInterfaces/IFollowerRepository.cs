namespace Backend.Core.EF;

public interface IFollowerRepository
{
    Task<(Response, FollowerDTO)> CreateAsync(FollowerCreateDTO user);
    Task<Response> UpdateAsync(FollowerUpdateDTO follower);
    Task<Response> DeleteByIdAsync(int id);
    Task<Response> DeleteByWhoAndWhomIds(int whoId, int whomId);
    Task<Option<FollowerDTO>> ReadById(int id);
    Task<Option<FollowerDTO>> ReadByWhoAndWhomId(int whoId, int whomId);
}