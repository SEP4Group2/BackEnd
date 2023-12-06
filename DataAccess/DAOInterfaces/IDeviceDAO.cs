using Domain.DTOs;
using Domain.Model;

namespace DataAccess.DAOInterfaces;

public interface IDeviceDAO
{
    public Task<Device> CreateAsync(DeviceRegistrationDTO newDevice);
    public Task<int> GetDeviceIdAsync(int deviceId);

    public Task<IEnumerable<int>> GetAllDeviceIdsAsync();

    public Task SetStatusByIdAsync(int deviceId);

}