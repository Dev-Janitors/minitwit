namespace Backend.Core.EF;

public record MessageDTO(
	int Id,
	int AuthorId,
	string Text,
	int PubDate,
    int Flagged
);

public record MessageCreateDTO {
	public int AuthorId {get; set;}
	public string? Text {get; set;}
	public int PubDate {get; set;}
}

public record MessageUpdateDTO : MessageCreateDTO {
    public int Id {get; set;}
	public int Flagged {get; set;}
}