namespace Backend.Core.EF;

public interface IFollowerRepository
{
    public Task<(Response, FollowerDTO)> CreateAsync(FollowerCreateDTO follow);
    public Task<Response> UpdateAsync(FollowerUpdateDTO follower);
    public Task<Response> ClearFollowers();
    public Task<Response> DeleteByIdAsync(int id);
    public Task<Response> DeleteByWhoAndWhomIds(int whoId, int whomId);
    public Task<Option<FollowerDTO>> ReadById(int id);
    public Task<Option<FollowerDTO>> ReadByWhoAndWhomId(int whoId, int whomId);
    public Task<IReadOnlyCollection<FollowerDTO>> ReadAllByWhomId(int whomId);
}