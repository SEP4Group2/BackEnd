using BackEndAPI.Controllers;
using BackEndAPI.Tests;
using DataAccess.DAOInterfaces;
using DataAccess.DAOs;
using Domain.DTOs;
using Domain.Model;
using Logic.Implementations;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

    [TestFixture]
    public class PlantPresetControllerTests : DatabaseTestFixture
    {

        private IPlantPresetManager presetManager;
        private IPlantPresetDAO presetDao;
        private PlantPresetController controller;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            presetDao = new PlantPresetDAO(Context);
            presetManager = new PlantPresetManagerImpl(presetDao);
            controller = new PlantPresetController(presetManager);
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedStatus()
        {
            // Arrange
            var user1 = new User
            {
                UserId = 1,
                Username = "testUser1",
                Password = "testPassword"
            };
            Context.Users.Add(user1);
            var plantPresetCreationDto = new PlantPresetCreationDTO
            {
                Temperature = 10,
                Humidity = 10,
                Moisture = 10,
                Name = "Preset",
                UVLight = 10,
                UserId = 1
            };

            var result = await controller.CreateAsync(plantPresetCreationDto);

            var createdResult = result.Result;
            Assert.That(createdResult, Is.TypeOf<CreatedResult>());

        }


        [Test]
        public async Task GetPlant_ShouldReturnOkStatus()
        {
            // Arrange
            ClearDatabase();
            // Add a sample plant preset to the database
            var plantPreset = new PlantPreset
            {
                Temperature = 30,
                Moisture = 20,
                Humidity = 20,
                UVLight = 80,
                Name = "MyPreset",
                UserId = 1
            };
            Context.Presets.Add(plantPreset);
            await Context.SaveChangesAsync();
            var result = await controller.GetPlant(plantPreset.PresetId);

            var createdResult = result.Result;
            Assert.That(createdResult, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetPresetsByUserId_ShouldReturnOkStatus()
        {
            // Arrange
            ClearDatabase();
            // Add a sample user and plant preset to the database
            var user = new User
            {
                UserId = 1,
                Username = "TestUsername",
                Password = "TestPassword"
            };
            var plantPreset = new PlantPreset
            {
                Temperature = 30,
                Moisture = 20,
                Humidity = 20,
                UVLight = 80,
                Name = "MyPreset",
                UserId = user.UserId // Link the plant preset to the user
            };
            Context.Users.Add(user);
            Context.Presets.Add(plantPreset);
            await Context.SaveChangesAsync();

            var result = await controller.GetPresetsByUserId(user.UserId);

            var createdResult = result.Result;
            Assert.That(createdResult, Is.TypeOf<OkObjectResult>());
        }



    }

