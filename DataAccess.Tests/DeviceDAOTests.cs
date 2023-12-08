﻿using DataAccess.DAOs;
using Domain.DTOs;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

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
    }
}