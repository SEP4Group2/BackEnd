using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Domain.Model;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Logic.Implementations;

public class DeviceManagerImpl:IDeviceManager
{
    private IDeviceDAO deviceDAO;

    public DeviceManagerImpl(IDeviceDAO deviceDAO)
    {
        this.deviceDAO = deviceDAO;
    }

    public async Task<Device> CreateAsync(DeviceRegistrationDTO newDevice)
    {
        return await deviceDAO.CreateAsync(newDevice);
    }
    
    public async Task<int> GetDeviceIdAsync(int deviceId)
    {
        return await deviceDAO.GetDeviceIdAsync(deviceId);
        

    }

    public async Task<IEnumerable<int>> GetAllDeviceIdsAsync()
    {
        return await deviceDAO.GetAllDeviceIdsAsync();
    }

    public async Task SetStatusByIdAsync(DeviceStatusDTO dto)
    {
        await deviceDAO.SetStatusById(dto);
    }
}
