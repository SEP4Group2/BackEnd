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

    public async Task<User> CreateAsync(UserCreationDto userCreationDto)
    {
       return await _userDao.CreateAsync(userCreationDto);
    }

    public async Task<IEnumerable<GetAllUsersDTO>> GetAllUsersAsync()
    {
        return await _userDao.GetAllUsersAsync();
    }
    
    
    
}