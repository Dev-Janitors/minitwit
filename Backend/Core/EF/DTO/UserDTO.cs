namespace  Backend.Core.EF;

public record UserDTO(
    int Id,
    
    [EmailAddress]
    string Email,

    string Username
);

public record UserCreateDTO {
    [Required]
    [EmailAddress]
    public string Email {get; set;}

    public string Username {get; set;}
}

public record UserLoginDTO {
    [Required]
    [EmailAddress]
    public string Email {get; set;}

    [Required]
    public string Username {get; set;}
}

public record UserJSON {
    public string email {get; set;}
    public string username {get; set;}
    //TODO: Add password
}

public record UserUpdateDTO : UserCreateDTO
{
    public int Id {get; set;}
}