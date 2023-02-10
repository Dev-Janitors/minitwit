namespace Backend.Infrastructure;

public class Follower
{
    public int Id {get;set;}

    public int who_id {get; set;}

    public int whom_id {get; set;}

    public Follower(int who_id, int whom_id)
    {
        this.who_id = who_id;
        this.whom_id = whom_id;
    }
}