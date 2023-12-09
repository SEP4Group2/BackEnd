using Domain.DTOs;
using Domain.Model;

namespace Logic.Interfaces;

public interface IDeviceManager
{
    public Task<Device> CreateAsync(DeviceRegistrationDTO newDevice);

    public Task<int> GetDeviceIdAsync(int deviceId);
    public Task<IEnumerable<int>> GetAllDeviceIdsAsync();

    public Task SetStatusByIdAsync(DeviceStatusDTO device);
}