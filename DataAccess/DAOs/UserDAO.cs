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

    public async Task<User> EditAsync(UserDTO user)
    {
        User updatedUser = _appContext.Users.First(u => u.UserId == user.UserId);
        if (user.Username != null) updatedUser.Username = user.Username;
        if (user.Password != null) updatedUser.Password = user.Password;
        try
        {
            _appContext.Users.Update(updatedUser);
            await _appContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return updatedUser;
    }

    public async Task RemoveAsync(int id)
    {
       
        User? user = await _appContext.Users.FindAsync(id);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var plantsToRemove = _appContext.Plants.Where(plant => plant.User.UserId == id).ToList();

        if (plantsToRemove.Count > 0)
        {
            foreach (Plant plant in plantsToRemove)
            {
                Device device = _appContext.Devices.First(d => d.Plant.Equals(plant));
                if (device != null)
                {
                    device.Plant.Equals(null);
                }
            }
            _appContext.Plants.RemoveRange(plantsToRemove);

            var presets = _appContext.Presets.Where(p => p.UserId == id);
            if ( presets != null)
            {
                foreach (var preset in presets)
                {
                    _appContext.Presets.Remove(preset);
                }
            }
        }

        _appContext.Users.Remove(user);
        await _appContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<User?>> GetAllUsersAsync()
    {
       
        var users = await _appContext.Users.ToListAsync();

        if (users == null || !users.Any())
        {
            throw new Exception("No users found");
        }
        return users.Select(user => new User(user.UserId,user.Username, user.Password));
    }
}
