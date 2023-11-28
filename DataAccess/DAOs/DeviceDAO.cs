using DataAccess.DAOInterfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccess.DAOs;

public class DeviceDAO : IDeviceDAO
{
    private readonly AppContext _appContext;

    public 
        DeviceDAO(AppContext appContext)
    {
        _appContext = appContext;
    }
    
    public async Task<Device> CreateAsync(Device newDevice)
    {
        try
        {

            EntityEntry<Device> device = await _appContext.Devices.AddAsync(newDevice);
            await _appContext.SaveChangesAsync();
            return device.Entity;

        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<int> GetDeviceIdAsync(int deviceId)
    {
        try
        {
            var device = await _appContext.Devices.FindAsync(deviceId);

            if (device != null)
            {
                return device.DeviceId;
            }
            else
            {
                throw new Exception("Device Id not found");
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    
}