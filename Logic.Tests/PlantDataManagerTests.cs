using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.DAOInterfaces;
using DataAccess.DAOs;
using Domain.DTOs;
using Domain.Model;
using Logic.Implementations;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Moq;
using NUnit.Framework;

[TestFixture]
public class PlantDataManagerTests
{
    private Mock<IPlantDataDAO> plantDataDaoMock;
    private PlantDataManagerImpl plantDataManagerImpl;
    
    [SetUp]
    public void Setup()
    {
        var notificationSenderMock = new Mock<INotificationSender>();
        // Mock PlantDataDao
        plantDataDaoMock = new Mock<IPlantDataDAO>();
        // Create an instance of the class under test, passing the mock dependency
        plantDataManagerImpl = new PlantDataManagerImpl(plantDataDaoMock.Object, notificationSenderMock.Object);
    }
    [Test]
    public async Task SaveAsync_ValidPlantData_ReturnsPlantData()
    {
        // Arrange
        var plantDataDaoMock = new Mock<IPlantDataDAO>();
        var notificationSenderMock = new Mock<INotificationSender>();
        var plantDataManager = new PlantDataManagerImpl(plantDataDaoMock.Object, notificationSenderMock.Object);

        var plantDataCreationDto = new PlantDataCreationDTO {Temperature = 1, Moisture = 2, UVLight = 3, Humidity = 4, DeviceId = 1, TankLevel = 12, TimeStamp = "5/12/2023 13.44.23"};
        var expectedPlantData = new PlantData {Temperature = 1, Moisture = 2, UVLight = 3, Humidity = 4, TankLevel = 12, TimeStamp = "5/12/2023 13.44.23"};

        //plantDataDaoMock.Setup(dao => dao.SaveAsync(It.IsAny<PlantDataCreationDTO>()))
          //              .ReturnsAsync(expectedPlantData);

        // Act
        //var result = await plantDataManager.SaveAsync(plantDataCreationDto);

        // Assert
        //Assert.NotNull(result);
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
    
    private List<PlantData> GetSamplePlantData()
    {
        return new List<PlantData>
        {
            new PlantData { TimeStamp = "2023-11-01 12:00:00", Humidity = 50, Temperature = 25, Moisture = 30, UVLight = 100 },
            new PlantData { TimeStamp = "2023-12-05 14:30:00", Humidity = 45, Temperature = 24, Moisture = 28, UVLight = 90 },
            new PlantData { TimeStamp = "2023-12-05 10:45:00", Humidity = 55, Temperature = 26, Moisture = 32, UVLight = 110 },
        };
    }
    
    private List<PlantData> GetSamplePlantDataOld()
    {
        return new List<PlantData>
        {
            new PlantData { TimeStamp = "2022-11-01 12:00:00", Humidity = 50, Temperature = 25, Moisture = 30, UVLight = 100 },
            new PlantData { TimeStamp = "2022-12-02 14:30:00", Humidity = 45, Temperature = 24, Moisture = 28, UVLight = 90 },
            new PlantData { TimeStamp = "2022-12-05 10:45:00", Humidity = 55, Temperature = 26, Moisture = 32, UVLight = 110 },
        };
    }
    
    private IEnumerable<IGrouping<DateTime, PlantData>> GetSampleGroupedData()
    {
        var plantDatas = GetSamplePlantData();
        return plantDatas.GroupBy(data => DateTime.ParseExact(data.TimeStamp, "yyyy-MM-dd HH:mm:ss", null).Date);
    }

    [Test]
    public void FilterPlantDataForLastSevenDays_ShouldFilterCorrectly()
    {

        var plantDatas = GetSamplePlantData();

        var result = plantDataManagerImpl.FilterPlantDataForLastSevenDays(plantDatas);
        
        Assert.IsNotNull(result);
        Assert.That(result.Count(), Is.EqualTo(2)); // only 2 are from last 7 days 
    }
    
    
    [Test]
    public void FilterPlantDataForLastSevenDays_WithOldPlantData()
    {
        var plantDatas = GetSamplePlantDataOld();

        var result = plantDataManagerImpl.FilterPlantDataForLastSevenDays(plantDatas);
        
        Assert.That(result, Is.Empty); // result should be empty as there was no recent sample data
    }
    
    [Test]
    public void GroupPlantDataByDate_ShouldGroupCorrectly()
    {
        var plantDatas = GetSamplePlantData();
        
        var result = plantDataManagerImpl.GroupPlantDataByDate(plantDatas);
        
        Assert.IsNotNull(result);
        Assert.That(result.Count(), Is.EqualTo(2)); // should result in two groupings 
        
        // check the count of items within a specific group
        foreach (var group in result)
        {
            if (group.Key == new DateTime(2023, 12, 05)) 
            {
                Assert.That(group.Count(), Is.EqualTo(2)); 
            }
        }
    }
    
    
    [Test]
    public void CalculateAverageValues_ShouldCalculateCorrectly()
    {
        var groupedData = GetSampleGroupedData();
        
        var result = plantDataManagerImpl.CalculateAverageValues(groupedData);
        
        Assert.IsNotNull(result);
        Assert.That(result.Count(), Is.EqualTo(2)); 
        
        // we should have two groups in the sample data
        var firstGroup = result.First(); // should have 1 plant data
        var secondGroup = result.Skip(1).First(); // should have 2 plant data 

        // assertions for the first group
        Assert.That(firstGroup.date, Is.EqualTo(new DateOnly(2023, 11, 01))); 
        Assert.That(firstGroup.avgHumidity, Is.EqualTo(50.0)); 
        Assert.That(firstGroup.avgTemperature, Is.EqualTo(25.0)); 
        Assert.That(firstGroup.avgMoisture, Is.EqualTo(30.0));
        Assert.That(firstGroup.avgUVLight, Is.EqualTo(100.0));

        // Assertions for the second group
        Assert.That(secondGroup.date, Is.EqualTo(new DateOnly(2023, 12, 05))); 
        Assert.That(secondGroup.avgHumidity, Is.EqualTo(50.0)); // (55 + 45 )/ 2
        Assert.That(secondGroup.avgTemperature, Is.EqualTo(25.0)); // (24 + 26) / 2
        Assert.That(secondGroup.avgMoisture, Is.EqualTo(30)); // (28 + 32) / 2
        Assert.That(secondGroup.avgUVLight, Is.EqualTo(100)); // (110 + 90) / 2
    }
    
    
    [Test]
    public async Task GetPlantDataForAnalytics_ShouldReturnCorrectResult()
    {
        int plantId = 1;
        List<PlantData> plantDatas = GetSamplePlantData();
        plantDataDaoMock.Setup(dao => dao.GetPlantDataByPlantIdAsync(plantId)).ReturnsAsync(plantDatas);
        
        var result = await plantDataManagerImpl.GetPlantDataForAnalytics(plantId);
        
        Assert.IsNotNull(result);
        Assert.That(result.Count, Is.EqualTo(1)); 
        Assert.That(result.First().date, Is.EqualTo(new DateOnly(2023,12,5)));
    }
    
    


}
