namespace Domain.DTOs;

public class GetAllUsersDTO
{
    public string Username { get; set; }
    public string Password { get; set; }

    public GetAllUsersDTO(string plantUsername, string plantPassword)
    {
        Username = plantUsername;
        Password = plantPassword;
    }
}