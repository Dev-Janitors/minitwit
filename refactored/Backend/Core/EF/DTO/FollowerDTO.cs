namespace Backend.Core.EF;

public record FollowerDTO(
    int Id,
    int who_id,
    int whom_id
);

public record FollowerCreateDTO{

    [Required]
    public int who_id {get;set;}

    [Required]
    public int whom_id {get;set;}

}

public record FollowerUpdateDTO : FollowerCreateDTO
{
    public int Id {get; set;}
}