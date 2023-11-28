using Domain.Model;

namespace DataAccess.DAOInterfaces;

public interface IDeviceDAO
{
    public Task<Device> CreateAsync(Device newDevice);
    public Task<int> GetDeviceIdAsync(int deviceId);

}