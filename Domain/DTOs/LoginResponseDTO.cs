using Domain.Model;

namespace Domain.DTOs;

public class LoginResponseDTO
{
    public string Token { get; set; }
    public User User { get; set; }
}