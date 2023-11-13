using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccess.DAOs;

public class UserDAO : IUserDAO
{
    private readonly AppContext _appContext;

    public UserDAO(AppContext appContext)
    {
        _appContext = appContext;
    }
    
    public async Task<User> CreateAsync(UserCreationDto userCreationDto)
    {
        try
        {

            var User = new User()
            {
                Username = userCreationDto.Username,
                Password = userCreationDto.Password
            };
            
            EntityEntry<User> newUser = await _appContext.Users.AddAsync(User);
            await _appContext.SaveChangesAsync();
            return newUser.Entity;

        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ICollection<GetAllUsersDTO>> GetAllUsersAsync()
    {
        return await _appContext.Users
            .Select(user => new GetAllUsersDTO (
                user.Username, user.Password))
            .ToListAsync();
    }
}