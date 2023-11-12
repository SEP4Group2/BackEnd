using Domain.DTOs;
using Domain.Model;

namespace DataAccess.DAOInterfaces;

public interface IUserDAO
{
     Task<User> CreateAsync(UserCreationDto userCreationDto);
}