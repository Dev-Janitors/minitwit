using System.ComponentModel.DataAnnotations.Schema;
namespace Backend.Infrastructure;

public class Message 
{
	[Key]
    public int Id {get; set;}

	public string Text {get; set;}

	public int PubDate {get; set;}

	public int Flagged {get; set;}

	public Message(string Text, int PubDate) {
		this.Text = Text;
		this.PubDate = PubDate;
	}
}