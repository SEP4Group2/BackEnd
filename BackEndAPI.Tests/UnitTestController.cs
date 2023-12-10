using BackEndAPI.Controllers;
using Domain.DTOs;
using Domain.Model;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace UnitTests.Controllers
{
    [TestFixture]
    public class PlantPresetControllerUnitTests
    {
        [Test]
        public async Task CreateAsync_ShouldReturnCreatedStatus()
        {
            // Arrange
            var plantPresetManagerMock = new Mock<IPlantPresetManager>();
            plantPresetManagerMock.Setup(manager => manager.CreateAsync(It.IsAny<PlantPresetCreationDTO>()))
                .ReturnsAsync(new PlantPreset { PresetId = 1 });

            var controller = new PlantPresetController(plantPresetManagerMock.Object);
            var plantPresetCreationDto = new PlantPresetCreationDTO();

            // Act
            var result = await controller.CreateAsync(plantPresetCreationDto);

            // Assert
            var createdResult = result.Result;
            Assert.That(createdResult, Is.TypeOf<CreatedResult>());
            
        }

        [Test]
        public async Task GetPlant_ShouldReturnOkStatus()
        {
            // Arrange
            var plantPresetManagerMock = new Mock<IPlantPresetManager>();
            plantPresetManagerMock.Setup(manager => manager.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new PlantPreset { PresetId = 1 });

            var controller = new PlantPresetController(plantPresetManagerMock.Object);

            // Act
            var result = await controller.GetPlant(1);

            // Assert
            var createdResult = result.Result;
            Assert.That(createdResult, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetPresetsByUserId_ShouldReturnOkStatus()
        {
            // Arrange
            var plantPresetManagerMock = new Mock<IPlantPresetManager>();
            plantPresetManagerMock.Setup(manager => manager.GetAllPresetsAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<PlantPreset> { new PlantPreset { PresetId = 1 } });

            var controller = new PlantPresetController(plantPresetManagerMock.Object);

            // Act
            var result = await controller.GetPresetsByUserId(1);

            // Assert
            var createdResult = result.Result;
            Assert.That(createdResult, Is.TypeOf<OkObjectResult>());
        }
    }
}
