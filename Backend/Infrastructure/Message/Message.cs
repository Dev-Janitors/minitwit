using System.ComponentModel.DataAnnotations.Schema;
using Backend.Core.EF;
namespace Backend.Infrastructure;

public class Message 
{
	[Key]
    public int Id {get; set;}

	public int AuthorId {get; set;}

	public string Text {get; set;}

	public long PubDate {get; set;}
	
	public int Flagged {get; set;}

	public Message(int AuthorId, string Text, long PubDate) {
		this.AuthorId = AuthorId;
		this.Text = Text;
		this.PubDate = PubDate;
	}

	public MessageDTO toDTO() =>
			new MessageDTO(Id, AuthorId, Text, PubDate, Flagged);

}
