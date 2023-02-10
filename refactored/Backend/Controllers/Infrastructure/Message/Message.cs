namespace Backend.Infrastructure;

public class Message 
{
    public int Id {get;set;}

    public int message_id {get; set;}

	public string text {get; set;}

	public int pub_date {get; set;}

	public int flagged {get; set;}

	public Message(int message_id, string text, int pub_date) {
		this.message_id = message_id;
		this.text = text;
		this.pub_date = pub_date;
	}

	public Message() {}
}