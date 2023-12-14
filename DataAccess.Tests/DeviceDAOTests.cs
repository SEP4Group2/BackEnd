using DataAccess.DAOs;
using Domain.DTOs;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Internal;

namespace Tests.DataAccess
{
    [TestFixture]
    public class DeviceDAOTests : DatabaseTestFixture
    {
        private DeviceDAO _deviceDao;

        [SetUp]
        public void SetUp()
        {
            _deviceDao = new DeviceDAO(Context);
        }
        

        [Test]
        public async Task CreateAsync_ShouldCreateDevice()
        {
            //Clear database
            ClearDatabase();
            
            await Context.SaveChangesAsync();
            // Arrange
            var newDevice = new DeviceRegistrationDTO
            {
                DeviceId = 1
            };

            // Act
            var createdDevice = await _deviceDao.CreateAsync(newDevice);

            // Assert
            Assert.IsNotNull(createdDevice);
            Assert.AreEqual(newDevice.DeviceId, createdDevice.DeviceId);
            Assert.IsNull(createdDevice.Plant);

        }

        [Test]
        public async Task GetDeviceIdAsync_ShouldReturnDeviceId()
        {
            //Clear database

            ClearDatabase();
            
            // Arrange
            var deviceId = 1;
            var device = new Device { DeviceId = deviceId, Status = true };
            Context.Devices.Add(device);
            await Context.SaveChangesAsync();

            // Act
            var result = await _deviceDao.GetDeviceIdAsync(deviceId);

            // Assert
            Assert.AreEqual(deviceId, result);


        }

        [Test]
        public void GetDeviceIdAsync_InvalidDeviceId_ShouldThrowException()
        {
            // Arrange
            var invalidDeviceId = 999;

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _deviceDao.GetDeviceIdAsync(invalidDeviceId));
        }
        
        
        [Test]
        public async Task GetAllDeviceIdsAsync_ShouldReturnAllDeviceIds()
        {
            // Arrange
            ClearDatabase();
            var deviceIds = new List<int> { 1, 2, 3 };
            foreach (var deviceId in deviceIds)
            {
                Context.Devices.Add(new Device { DeviceId = deviceId, Status = true });
            }
            await Context.SaveChangesAsync();

            // Act
            var result = await _deviceDao.GetAllDeviceIdsAsync();

            // Assert
            Assert.IsNotNull(result);
            CollectionAssert.AreEquivalent(deviceIds, result);
        }

        [Test]
        public async Task SetStatusById_ShouldUpdateDeviceStatus()
        {
            // Arrange
            ClearDatabase();
            var deviceId = 1;
            var device = new Device { DeviceId = deviceId, Status = true };
            Context.Devices.Add(device);
            await Context.SaveChangesAsync();

            var deviceStatusDTO = new DeviceStatusDTO { DeviceId = deviceId, Status = false };

            // Act
            await _deviceDao.SetStatusById(deviceStatusDTO);

            // Assert
            var updatedDevice = await Context.Devices.FindAsync(deviceId);
            Assert.IsNotNull(updatedDevice);
            Assert.AreEqual(deviceStatusDTO.Status, updatedDevice.Status);
        }
        
        [Test]
        public async Task GetDeviceIdAsync_ShouldThrowExceptionForInvalidDeviceId()
        {
            // Arrange
            ClearDatabase();

            var invalidDeviceId = 1; 

            // Act
            async Task Act() => await _deviceDao.GetDeviceIdAsync(invalidDeviceId);

            // Assert
            Assert.ThrowsAsync<Exception>(Act);
        }

        [Test]
        public async Task SetStatusById_ShouldThrowExceptionForInvalidDeviceId()
        {
            // Arrange
            ClearDatabase();

            var invalidDeviceStatusDTO = new DeviceStatusDTO
            {
                DeviceId = 1,
                Status = false
            };

            // Act
            async Task Act() => await _deviceDao.SetStatusById(invalidDeviceStatusDTO);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(Act);
        }

    }
    
    
}
