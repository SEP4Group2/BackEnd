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

        var plantDataCreationDto = new PlantDataCreationDTO {Temperature = 1, Moisture = 2, UVLight = 3, Humidity = 4, DeviceId = 1, TankLevel = 12, TimeStamp = "5/12/2023 13.44.23"};
        var expectedPlantData = new PlantData {Temperature = 1, Moisture = 2, UVLight = 3, Humidity = 4, TankLevel = 12, TimeStamp = "5/12/2023 13.44.23"};

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
            Humidity = 1,
            Moisture = 1,
            Temperature = 1,
            UVLight = 1
        };

        // Set optimal preset values here for testing
        var optimalPreset = new PlantPreset
        {
            Humidity = 50,
            Temperature = 25,
            UVLight = 300,
            Moisture = 40
        };

        var plantDevice = new Device
        {
            Plant = new Plant
            {
                PlantPreset = optimalPreset,
                Name = "TestPlant",
                User = new User { UserId = 123 }
            }
        };

        plantData.PlantDevice = plantDevice;

        // Act
        await plantDataManager.CheckDataWithPlantPreset(plantData);

        // Assert
        notificationSenderMock.Verify(
            sender => sender.SendNotification(It.IsAny<NotificationRequestDTO>()),
            Times.Exactly(1) // Assuming all conditions are met
        );
    }


    [Test]
    public async Task FetchPlantDataAsync_ValidUserId_ReturnsPlantDataList()
    {
        // Arrange
        var plantDataDaoMock = new Mock<IPlantDataDAO>();
        var notificationSenderMock = new Mock<INotificationSender>();
        var plantDataManager = new PlantDataManagerImpl(plantDataDaoMock.Object, notificationSenderMock.Object);

        var userId = 1;
        var expectedPlantDataList = new List<PlantData> {new PlantData()
        {
            Humidity = 1, Temperature = 1, Moisture = 1, TankLevel = 1, TimeStamp = "5/12/2023 13.44.22", UVLight = 1
        },new PlantData()
            {
            Humidity = 1, Temperature = 2, Moisture = 3, TankLevel = 4, TimeStamp = "5/12/2023 13.44.22", UVLight = 5
        }};

        plantDataDaoMock.Setup(dao => dao.FetchPlantDataAsync(It.IsAny<int>()))
                        .ReturnsAsync(expectedPlantDataList);

        // Act
        var result = await plantDataManager.FetchPlantDataAsync(userId);

        // Assert
        Assert.NotNull(result);
    }
}
