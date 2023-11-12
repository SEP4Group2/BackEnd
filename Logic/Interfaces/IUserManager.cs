using Domain.DTOs;
using Domain.Model;

namespace Logic.Interfaces;

public interface IUserManager
{
    Task<User> CreateAsync(UserCreationDto userCreationDto);
}