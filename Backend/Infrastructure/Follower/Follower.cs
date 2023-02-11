namespace Backend.Infrastructure;

public class Follower
{
    public int Id {get; private set;}

    public int WhoId {get; set;}

    public int WhomId {get; set;}

    public Follower(int WhoId, int WhomId)
    {
        this.WhoId = WhoId;
        this.WhomId = WhomId;
    }
}