using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccess.DAOs;

public class DeviceDAO : IDeviceDAO
{
    private readonly AppContext _appContext;

    public DeviceDAO(AppContext appContext)
    {
        _appContext = appContext;
    }
    
    public async Task<Device> CreateAsync(DeviceRegistrationDTO newDevice)
    {
        try
        {
            var device = new Device()
            {
                DeviceId = newDevice.DeviceId,
                Status = true
            };
            device.Status = false;
            EntityEntry<Device> deviceEntity = await _appContext.Devices.AddAsync(device);
            await _appContext.SaveChangesAsync();
            return deviceEntity.Entity;
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

    public async Task<IEnumerable<int>> GetAllDeviceIdsAsync()
    {
        try
        {
            var devicesIds = await _appContext.Devices.Select(d => d.DeviceId).ToListAsync();
            return devicesIds;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SetStatusByIdAsync(int id)
    {
        try
        {
            var device = await _appContext.Devices.FindAsync(id);

            if (device != null)
            {
                device.Status = true; 
                await _appContext.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}