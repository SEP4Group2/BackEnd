using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Domain.Model;
using Logic.Implementations;
using Logic.Interfaces;
using Moq;
using NUnit.Framework;

[TestFixture]
public class PlantDataManagerTests
{
    [Test]
    public async Task SaveAsync_ValidPlantData_ReturnsPlantData()
    {
        // Arrange
        var plantDataDaoMock = new Mock<IPlantDataDAO>();
        var notificationSenderMock = new Mock<INotificationSender>();
        var plantDataManager = new PlantDataManagerImpl(plantDataDaoMock.Object, notificationSenderMock.Object);

        var plantDataCreationDto = new PlantDataCreationDTO { /* provide valid data */ };
        var expectedPlantData = new PlantData { /* create a valid PlantData object */ };

        plantDataDaoMock.Setup(dao => dao.SaveAsync(It.IsAny<PlantDataCreationDTO>()))
                        .ReturnsAsync(expectedPlantData);

        // Act
        var result = await plantDataManager.SaveAsync(plantDataCreationDto);

        // Assert
        Assert.NotNull(result);
        // Add more assertions based on your requirements
    }

    [Test]
    public async Task CheckDataWithPlantPreset_OutsideOptimalRange_SendsNotification()
    {
        // Arrange
        var plantDataDaoMock = new Mock<IPlantDataDAO>();
        var notificationSenderMock = new Mock<INotificationSender>();
        var plantDataManager = new PlantDataManagerImpl(plantDataDaoMock.Object, notificationSenderMock.Object);

        var plantData = new PlantData
        {
            Humidity = 60, // Assume the optimal preset is 50, and the difference is more than 50
            Temperature = 25,
            UVLight = 300,
            Moisture = 40
        };

        // Act
        await plantDataManager.CheckDataWithPlantPreset(plantData);

        // Assert
        notificationSenderMock.Verify(sender => sender.SendNotification(It.IsAny<NotificationRequestDTO>()), Times.Exactly(1));
        // Add more assertions based on your requirements
    }

    [Test]
    public async Task FetchPlantDataAsync_ValidUserId_ReturnsPlantDataList()
    {
        // Arrange
        var plantDataDaoMock = new Mock<IPlantDataDAO>();
        var notificationSenderMock = new Mock<INotificationSender>();
        var plantDataManager = new PlantDataManagerImpl(plantDataDaoMock.Object, notificationSenderMock.Object);

        var userId = 1;
        var expectedPlantDataList = new List<PlantData> { /* create a list of PlantData objects */ };

        plantDataDaoMock.Setup(dao => dao.FetchPlantDataAsync(It.IsAny<int>()))
                        .ReturnsAsync(expectedPlantDataList);

        // Act
        var result = await plantDataManager.FetchPlantDataAsync(userId);

        // Assert
        Assert.NotNull(result);
        // Add more assertions based on your requirements
    }
}
