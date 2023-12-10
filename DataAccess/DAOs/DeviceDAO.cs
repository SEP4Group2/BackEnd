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

    public async Task SetStatusById(DeviceStatusDTO device)
    {
        try
        {
            var Changeddevice =  _appContext.Devices.First(d=> d.DeviceId==device.DeviceId);

            if (Changeddevice != null)
            {
                device.Status = device.Status; 
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