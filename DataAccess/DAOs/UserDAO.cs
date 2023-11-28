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
    
    public async Task<User> CreateAsync(UserDTO userCreationDto)
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

    public async Task<IEnumerable<User?>> GetAllUsersAsync()
    {
       
        var users = await _appContext.Users.ToListAsync();

        if (users == null || !users.Any())
        {
            throw new Exception("No users found");
        }
        Console.WriteLine("done");
        return users.Select(user => new User(user.UserId,user.Username, user.Password));
    }
}