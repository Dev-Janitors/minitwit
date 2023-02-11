namespace Backend.Infrastructure;

public class User 
{
    public int Id {get; private set;}
    
    [EmailAddress]
    public string Email {get; set;}

    public string Username {get; set;}


    public User(string Email, string Username)
    {
        this.Username = Username;
        this.Email = Email;
    }

    
}