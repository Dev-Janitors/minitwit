namespace Backend.Core.EF;

public record MessageDTO(
	int Id,
	int AuthorId,
	string Text,
	long PubDate,
    int Flagged
);

public record MessageCreateDTO {
	public int AuthorId {get; set;}
	public string? Text {get; set;}
	public long PubDate {get; set;}
}

public record MessageUpdateDTO : MessageCreateDTO {
    public int Id {get; set;}
	public int Flagged {get; set;}
}

public record MessageCreateJson {
	public string? content {get; set;}
}

public record AllMessages {
	public string? content {get; set;} //actual message
	public string? user {get; set;} //username
	public long pubDate {get; set;}
}