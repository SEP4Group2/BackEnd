using DataAccess.DAOs;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Tests.DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTOs;
using NUnit.Framework;

    [TestFixture]
    public class PlantDAOTests : DatabaseTestFixture
    {
        private PlantDAO _plantDao;

        [SetUp]
        public void SetUp()
        {
            _plantDao = new PlantDAO(Context);
        }

        [Test]
        public async Task CreateAsync_ShouldCreatePlant()
        {
            //Clear database
           ClearDatabase();
            
            await Context.SaveChangesAsync();
            // Arrange
            var user1 = new User
            {
                UserId = 1, 
                Username = "testUser1", 
                Password = "testPassword"
            };
        
            var plantPreset = new PlantPreset
            {
                PresetId = 1,
                UserId = 1, 
                Name = "TestPreset",
                Humidity = 50,
                UVLight = 500,
                Moisture = 30,
                Temperature = 25,
            };
            var device = new Device 
                { 
                    DeviceId = 1,
                    Status = true,
                };
            
            Context.Devices.Add(device);
            Context.Users.Add(user1);
            Context.Presets.Add(plantPreset);
            await Context.SaveChangesAsync();
            
            var plantCreationDto = new PlantCreationDTO
            {
                Location = "TestLocation",
                PlantPresetId = 1, 
                UserId = 1,
                DeviceId = 1, 
                Name = "TestPlant",
                IconId = 1
            };

            // Act
            var createdPlant = await _plantDao.CreateAsync(plantCreationDto);

            // Assert
            Assert.IsNotNull(createdPlant);
            Assert.AreEqual(plantCreationDto.Location, createdPlant.Location);
            Assert.AreEqual(plantCreationDto.Name, createdPlant.Name);
            Assert.IsNotNull(createdPlant.PlantPreset);
            Assert.IsNotNull(createdPlant.User);
            

        }

        [Test]
        public async Task GetAsync_ShouldReturnPlant()
        {
            //Clear database
            ClearDatabase();
            
            await Context.SaveChangesAsync();
            // Arrange
            var plantId = 1; 
            var user1 = new User
            {
                UserId = 1, 
                Username = "testUser1", 
                Password = "testPassword"
            };
        
            var plantPreset = new PlantPreset
            {
                PresetId = 1,
                UserId = 1, 
                Name = "TestPreset",
                Humidity = 50,
                UVLight = 500,
                Moisture = 30,
                Temperature = 25,
            };
            
            
            var plant = new Plant
            {
                PlantId = plantId,
                User = user1,
                Location = "testLocation",
                Name = "testPreset",
                PlantPreset = plantPreset,
                IconId = 1
            };
            Context.Users.Add(user1);
            Context.Presets.Add(plantPreset);
            Context.Plants.Add(plant);
            
            await Context.SaveChangesAsync();

            // Act
            var result = await _plantDao.GetAsync(plantId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(plantId, result.PlantId);
        }

        [Test]
        public async Task GetAllPlantsAsync_ShouldReturnListOfPlants()
        {
            //Clear database
            ClearDatabase();
            
            await Context.SaveChangesAsync();
            //Arrange 

            var userId = 1;
            
            var user1 = new User
            {
                UserId = userId, 
                Username = "testUser1", 
                Password = "testPassword"
            };
        
            var plantPreset = new PlantPreset
            {
                PresetId = 1,
                UserId = userId, 
                Name = "TestPreset",
                Humidity = 50,
                UVLight = 500,
                Moisture = 30,
                Temperature = 25,
            };

            var plant1 = new Plant
            {
                PlantId = 1,
                User = user1,
                Location = "testLocation",
                Name = "testPreset",
                PlantPreset = plantPreset,
                IconId = 1

            };
            
            var plant2 = new Plant
            {
                PlantId = 2,
                User = user1,
                Location = "testLocation",
                Name = "testPreset",
                PlantPreset = plantPreset,
                IconId = 1
            };
            
            var plant3 = new Plant
            {
                PlantId = 3,
                User = user1,
                Location = "testLocation",
                Name = "testPreset",
                PlantPreset = plantPreset,
                IconId = 1,
            };

            var device1 = new Device
            {
                DeviceId = 1,
                Plant = plant1,
                Status = true
            };
            
            var device2 = new Device
            {
                DeviceId = 2,
                Plant = plant2,
                Status = true
            };
            
            var device3 = new Device
            {
                DeviceId = 3,
                Plant = plant3,
                Status = true
            };
            
            
            Context.Users.Add(user1);
            Context.Presets.Add(plantPreset);
            Context.Plants.Add(plant1);
            Context.Plants.Add(plant2);
            Context.Plants.Add(plant3);
            Context.Devices.Add(device1);
            Context.Devices.Add(device2);
            Context.Devices.Add(device3);

            Context.SaveChangesAsync();

            // Act
            var plants = await _plantDao.GetAllPlantsAsync(userId);

            // Assert
            Assert.IsNotNull(plants);
            Assert.IsInstanceOf<List<GetAllPlantsDTO>>(plants);

        }

        [Test]
        public async Task RemoveAsync_ShouldRemovePlant()
        {
            //Clear database
            ClearDatabase();
            
            await Context.SaveChangesAsync();
            // Arrange
            var plantIdToRemove = 1; 
            var user1 = new User
            {
                UserId = 1, 
                Username = "testUser1", 
                Password = "testPassword"
            };
        
            var plantPreset = new PlantPreset
            {
                PresetId = 1,
                UserId = 1, 
                Name = "TestPreset",
                Humidity = 50,
                UVLight = 500,
                Moisture = 30,
                Temperature = 25,
            };

            var plant = new Plant
            {
                PlantId = plantIdToRemove,
                User = user1,
                Location = "testLocation",
                Name = "testPreset",
                PlantPreset = plantPreset,
                IconId = 1
            };

            var device = new Device
            {
                DeviceId = 1,
                Plant = plant,
                Status = true
            };
            
            Context.Users.Add(user1);
            Context.Presets.Add(plantPreset);
            Context.Plants.Add(plant);
            Context.Devices.Add(device);
            
            await Context.SaveChangesAsync();
            

            // Act
            await _plantDao.RemoveAsync(plantIdToRemove);

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await _plantDao.GetAsync(plantIdToRemove));
        }

        [Test]
        public async Task EditAsync_ShouldEditPlant()
        {
            //Clear database
            ClearDatabase();
            
            await Context.SaveChangesAsync();
            // Arrange
            var user1 = new User
            {
                UserId = 1, 
                Username = "testUser1", 
                Password = "testPassword"
            };
        
            var plantPreset = new PlantPreset
            {
                PresetId = 1,
                UserId = 1, 
                Name = "TestPreset",
                Humidity = 50,
                UVLight = 500,
                Moisture = 30,
                Temperature = 25,
            };

            var plant = new Plant
            {
                PlantId = 1,
                User = user1,
                Location = "testLocation",
                Name = "testPreset",
                PlantPreset = plantPreset,
                IconId = 1
            };
            Context.Users.Add(user1);
            Context.Presets.Add(plantPreset);
            Context.Plants.Add(plant);
            
            await Context.SaveChangesAsync();

            
            var editPlantDto = new EditPlantDTO
            {
                PlantId = 1, 
                Location = "EditedLocation",
                Name = "EditedPlant"
            };

            // Act
            var editedPlant = await _plantDao.EditAsync(editPlantDto);

            // Assert
            Assert.IsNotNull(editedPlant);
            Assert.AreEqual(editPlantDto.Location, editedPlant.Location);
            Assert.AreEqual(editPlantDto.Name, editedPlant.Name);
            
        }
        
        [Test]
        public async Task CreateAsync_ShouldThrowExceptionForNonExistingPreset()
        {
            // Arrange
            ClearDatabase();

            var plantCreationDto = new PlantCreationDTO
            {
                PlantPresetId = 1, 
            };

            // Act
            async Task Act() => await _plantDao.CreateAsync(plantCreationDto);

            // Assert
            Assert.ThrowsAsync<Exception>(Act);
        }

        [Test]
        public async Task CreateAsync_ShouldThrowExceptionForInvalidUserId()
        {
            // Arrange
            ClearDatabase();

            var plantCreationDto = new PlantCreationDTO
            {
                UserId = 1, 
            };

            // Act
            async Task Act() => await _plantDao.CreateAsync(plantCreationDto);

            // Assert
            Assert.ThrowsAsync<Exception>(Act);
        }

        [Test]
        public async Task CreateAsync_ShouldThrowExceptionForInvalidDeviceId()
        {
            // Arrange
            ClearDatabase();

            var plantCreationDto = new PlantCreationDTO
            {
                DeviceId = 999, 
            };

            // Act
            async Task Act() => await _plantDao.CreateAsync(plantCreationDto);

            // Assert
            Assert.ThrowsAsync<Exception>(Act);
        }

        [Test]
        public async Task RemoveAsync_ShouldThrowExceptionForInvalidPlantId()
        {
            // Arrange
            ClearDatabase();

            var plantIdToRemove = 999; 

            // Act
            async Task Act() => await _plantDao.RemoveAsync(plantIdToRemove);

            // Assert
            Assert.ThrowsAsync<Exception>(Act);
        }

    }
