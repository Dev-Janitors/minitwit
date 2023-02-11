namespace Backend.Core.EF;

public record FollowerDTO(
    int Id,
    int WhoId,
    int WhomId
);

public record FollowerCreateDTO{

    [Required]
    public int WhoId {get;set;}

    [Required]
    public int WhomId {get;set;}

}

public record FollowerUpdateDTO : FollowerCreateDTO
{
    public int Id {get; set;}
}