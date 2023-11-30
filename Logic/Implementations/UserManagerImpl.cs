using System.Net.Http.Json;
using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Domain.Model;
using Logic.Interfaces;

namespace Logic.Implementations;

public class UserManagerImpl : IUserManager
{
    private IUserDAO _userDao;


    public UserManagerImpl(IUserDAO plantUserDao)
    {
        _userDao = plantUserDao;
    }

    public async Task<User> CreateAsync(UserDTO userCreationDto)
    {
        return await _userDao.CreateAsync(userCreationDto);
    }

    public async Task<IEnumerable<User?>> GetAllUsersAsync()
    {
        return await _userDao.GetAllUsersAsync();
    }


    public async Task<User?> ValidateUser(UserDTO dto)
    {
        IEnumerable<User?> users = await _userDao.GetAllUsersAsync();
        User? existingUser = users.FirstOrDefault(u =>
            u.Username.Equals(dto.Username, StringComparison.OrdinalIgnoreCase));

        if (existingUser == null)
        {
            throw new Exception("User not found");
        }

        if (!existingUser.Password.Equals(dto.Password))
        {
            throw new Exception("Password mismatch");
        }
        return existingUser;
    }
    
}
