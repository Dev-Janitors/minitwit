namespace Backend.Infrastructure;

public class Message 
{
    public int Id {get;set;}

    public int MessageId {get; set;}

	public string Text {get; set;}

	public int PubDate {get; set;}

	public int Flagged {get; set;}

	public Message(int MessageId, string Text, int PubDate) {
		this.MessageId = MessageId;
		this.Text = Text;
		this.PubDate = PubDate;
	}

	public Message() {}
}