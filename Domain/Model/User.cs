using System.ComponentModel.DataAnnotations;

namespace Domain.Model;

public class User
{
    [Key] 
    public int UserId { get; set; }
    
    public string Username { get; set; }
    
    public string Password { get; set; }

    public User(int id,string plantUsername, string plantPassword)
    {
        UserId = id;
        Username = plantUsername;
        Password = plantPassword;
    }

    public User()
    {
    }
}