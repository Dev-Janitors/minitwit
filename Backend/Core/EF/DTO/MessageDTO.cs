namespace Backend.Core.EF;

public record MessageDTO(
	int Id,
	string Text,
	int PubDate,
    int Flagged
);

public record MessageCreateDTO {
	public string? Text {get; set;}
	public int PubDate {get; set;}
}

public record MessageUpdateDTO : MessageCreateDTO {
    public int Id {get; set;}
}