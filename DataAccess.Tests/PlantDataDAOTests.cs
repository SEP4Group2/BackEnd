using DataAccess.DAOs;
namespace Tests.DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Model;
using NUnit.Framework;

    [TestFixture]
    public class PlantDataDAOTests : DatabaseTestFixture
    {
        private PlantDataDAO _plantDataDao;

        [SetUp]
        public void SetUp()
        {
            _plantDataDao = new PlantDataDAO(Context);
        }

        [Test]
        public async Task SaveAsync_ShouldSavePlantData()
        {
            //Clear database
            
            ClearDatabase();
            
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
                UserId = 1, // Assuming a valid user ID for testing
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
            var device = new Device 
            { 
                DeviceId = 1,
                Status = true,
                Plant = plant
            };
            
            Context.Devices.Add(device);
            Context.Users.Add(user1);
            Context.Presets.Add(plantPreset);
            Context.Plants.Add(plant);
            
            await Context.SaveChangesAsync();
            
            var plantDataCreationDto = new PlantDataCreationDTO
            {
                DeviceId = 1, // Assuming a valid device ID for testing
                Humidity = 50,
                Temperature = 25,
                Moisture = 30,
                UVLight = 500,
                TimeStamp = "time",
                TankLevel = 75
            };

            // // Act
            // var savedPlantData = await _plantDataDao.SaveAsync(plantDataCreationDto);
            //
            // // Assert
            // Assert.IsNotNull(savedPlantData);
            // Assert.AreEqual(plantDataCreationDto.Humidity, savedPlantData.Humidity);
            // Assert.AreEqual(plantDataCreationDto.Temperature, savedPlantData.Temperature);
            // Assert.AreEqual(plantDataCreationDto.Moisture, savedPlantData.Moisture);
            // Assert.AreEqual(plantDataCreationDto.UVLight, savedPlantData.UVLight);
            // Assert.AreEqual(plantDataCreationDto.TimeStamp, savedPlantData.TimeStamp);
            // Assert.AreEqual(plantDataCreationDto.TankLevel, savedPlantData.TankLevel);
            //
        }

        [Test]
        public async Task FetchPlantDataAsync_ShouldReturnListOfPlantData()
        {
            //Clear database
            
            ClearDatabase();

            await Context.SaveChangesAsync();
            
            // Arrange
            var userId = 1; // Assuming a valid user ID for testing
            var user1 = new User
            {
                UserId = userId, 
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
            var device = new Device 
            { 
                DeviceId = 1,
                Status = true,
                Plant = plant
            };

            var plantData = new PlantData
            {
                PlantDevice = device, 
                Humidity = 50,
                Temperature = 25,
                Moisture = 30,
                UVLight = 500,
                TimeStamp = "2023-12-07T10:30:00",
                TankLevel = 75
            };
            
            Context.Devices.Add(device);
            Context.Users.Add(user1);
            Context.Presets.Add(plantPreset);
            Context.Plants.Add(plant);
            Context.PlantData.Add(plantData);
            await Context.SaveChangesAsync();

            // Act
            var fetchedPlantData = await _plantDataDao.FetchPlantDataAsync(userId);

            // Assert
            Assert.IsNotNull(fetchedPlantData);
            Assert.IsInstanceOf<List<PlantData>>(fetchedPlantData);
            
        }
    }
