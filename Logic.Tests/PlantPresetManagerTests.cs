using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Domain.Model;
using Logic.Implementations;
using Moq;

[TestFixture]
public class PlantPresetManagerImplTests
{
    [Test]
    public async Task CreateAsync_ValidPlantPreset_ReturnsPlantPreset()
    {
        // Arrange
        var plantPresetDaoMock = new Mock<IPlantPresetDAO>();
        var plantPresetManager = new PlantPresetManagerImpl(plantPresetDaoMock.Object);

        var plantPresetCreationDto = new PlantPresetCreationDTO {Humidity = 10, Temperature = 30, UVLight = 10, Moisture = 20, UserId = 1, Name = "Preset"};
        var expectedPlantPreset = new PlantPreset{Humidity = 10, Temperature = 30, UVLight = 10, Moisture = 20, UserId = 1, Name = "Preset" };

        plantPresetDaoMock.Setup(dao => dao.CreateAsync(It.IsAny<PlantPresetCreationDTO>()))
                          .ReturnsAsync(expectedPlantPreset);

        // Act
        var result = await plantPresetManager.CreateAsync(plantPresetCreationDto);

        // Assert
        Assert.NotNull(result);
    }

    [Test]
    public async Task GetByIdAsync_ValidPresetId_ReturnsPlantPreset()
    {
        // Arrange
        var plantPresetDaoMock = new Mock<IPlantPresetDAO>();
        var plantPresetManager = new PlantPresetManagerImpl(plantPresetDaoMock.Object);

        var presetId = 1;
        var expectedPlantPreset = new PlantPreset { Humidity = 10, Temperature = 30, UVLight = 10, Moisture = 20, UserId = 1, Name = "Preset" };

        plantPresetDaoMock.Setup(dao => dao.GetAsync(It.IsAny<int>()))
                          .ReturnsAsync(expectedPlantPreset);

        // Act
        var result = await plantPresetManager.GetByIdAsync(presetId);

        // Assert
        Assert.NotNull(result);
        // Add more assertions based on your requirements
    }

    [Test]
    public async Task GetAllPresetsAsync_ValidUserId_ReturnsPlantPresetList()
    {
        // Arrange
        var plantPresetDaoMock = new Mock<IPlantPresetDAO>();
        var plantPresetManager = new PlantPresetManagerImpl(plantPresetDaoMock.Object);

        var userId = 1;
        var expectedPlantPresetsList = new List<PlantPreset> {new PlantPreset()
        {
            Humidity = 10, Temperature = 20, UserId = 1, Name = "Preset2", Moisture = 20, UVLight = 15
        }, new PlantPreset(){Humidity = 30, UVLight = 20, UserId = 1, Name = "Preset5", Temperature = 30, Moisture = 20} };

        plantPresetDaoMock.Setup(dao => dao.GetAllPresetsAsync(It.IsAny<int>()))
                          .ReturnsAsync(expectedPlantPresetsList);

        // Act
        var result = await plantPresetManager.GetAllPresetsAsync(userId);

        // Assert
        Assert.NotNull(result);
    }
}
