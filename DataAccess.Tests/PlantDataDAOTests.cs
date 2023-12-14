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
        public async Task SaveAsync_ShouldSavePlantDataList()
        {
            // Arrange
            ClearDatabase();

            var user1 = new User
            {
                UserId = 1,
                Username = "testUser",
                Password = "testPassword"
            };

            var plantPreset = new PlantPreset
            {
                PresetId = 1,
                UserId = 1,
                Name = "TestPreset",
                Humidity = 50,
                UVLight = 800,
                Moisture = 30,
                Temperature = 23,
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

            var plantDataList = new PlantDataCreationListDTO
            {
                PlantDataApi = new List<PlantDataCreationDTO>
                {
                    new PlantDataCreationDTO
                    {
                        DeviceId = 1, 
                        Humidity = 50,
                        Temperature = 25,
                        Moisture = 30,
                        UVLight = 500,
                        TimeStamp = "time",
                        TankLevel = 75
                    },
                    new PlantDataCreationDTO
                    {
                        DeviceId = 1, 
                        Humidity = 40,
                        Temperature = 25,
                        Moisture = 30,
                        UVLight = 300,
                        TimeStamp = "time",
                        TankLevel = 75
                    }
                }
            };

            // Act
            var savedPlantData = await _plantDataDao.SaveAsync(plantDataList);

            // Assert
            Assert.IsNotNull(savedPlantData);
        }


        [Test]
        public async Task FetchPlantDataAsync_ShouldReturnListOfPlantData()
        {
            //Clear database
            
            ClearDatabase();

            await Context.SaveChangesAsync();
            
            // Arrange
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
        
                [Test]
        public async Task GetPlantDataByPlantIdAsync_ShouldReturnPlantDataForSpecifiedPlantId()
        {
            // Arrange
            ClearDatabase();

            var user1 = new User
            {
                UserId = 1,
                Username = "testUser",
                Password = "testPassword"
            };

            var plantPreset = new PlantPreset
            {
                PresetId = 1,
                UserId = 1,
                Name = "TestPreset",
                Humidity = 50,
                UVLight = 800,
                Moisture = 30,
                Temperature = 23,
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
            var fetchedPlantData = await _plantDataDao.GetPlantDataByPlantIdAsync(plant.PlantId);

            // Assert
            Assert.IsNotNull(fetchedPlantData);
            Assert.AreEqual(1, fetchedPlantData.Count);
            Assert.AreEqual(plantData.Humidity, fetchedPlantData[0].Humidity);
        }

        
        [Test]
        public async Task SaveAsync_ShouldThrowExceptionForNullDevice()
        {
            // Arrange
            ClearDatabase();

            var plantDataList = new PlantDataCreationListDTO
            {
                PlantDataApi = new List<PlantDataCreationDTO>
                {
                    new PlantDataCreationDTO
                    {
                        DeviceId = 1, 
                        Humidity = 50,
                        Temperature = 25,
                        Moisture = 30,
                        UVLight = 500,
                        TimeStamp = "time",
                        TankLevel = 75
                    },
                }
            };

            // Act
            async Task Act() => await _plantDataDao.SaveAsync(plantDataList);

            // Assert
            Assert.ThrowsAsync<Exception>(Act);
        }

        [Test]
        public async Task GetPlantDataByPlantIdAsync_ShouldThrowExceptionForInvalidPlantId()
        {
            // Arrange
            ClearDatabase();

            var invalidPlantId = 999; // Invalid plant ID

            // Act
            async Task Act() => await _plantDataDao.GetPlantDataByPlantIdAsync(invalidPlantId);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(Act);
        }
        
    }
