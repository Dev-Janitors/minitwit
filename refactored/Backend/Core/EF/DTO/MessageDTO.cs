namespace Backend.Core.EF;

public record MessageDTO(
	int Id,
	int message_id,
	string text,
	int pub_date,
    int flagged
);

public record MessageCreateDTO {
	public int message_id {get; set;}
	public string? text {get; set;}
	public int pub_date {get; set;}
}

public record MessageUpdateDTO : MessageCreateDTO {
    public int Id {get; set;}
}