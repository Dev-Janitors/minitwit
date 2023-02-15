using System.ComponentModel.DataAnnotations.Schema;
namespace Backend.Infrastructure;

public class Message 
{
	[Key]
    public int Id {get; set;}

	public int AuthorId {get; set;}

	public string Text {get; set;}

	public int PubDate {get; set;}

	public int Flagged {get; set;}

	public Message(int AuthorId, string Text, int PubDate) {
		this.AuthorId = AuthorId;
		this.Text = Text;
		this.PubDate = PubDate;
	}
}