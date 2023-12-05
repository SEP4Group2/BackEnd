using Domain.DTOs;
using Domain.Model;

namespace Logic.Interfaces;

public interface IUserManager
{
    Task<User> CreateAsync(UserDTO userCreationDto);

    Task<IEnumerable<User?>> GetAllUsersAsync();
    Task<User?> ValidateUser(UserDTO dto);

    Task<User> EditAsync(UserDTO dto);
}