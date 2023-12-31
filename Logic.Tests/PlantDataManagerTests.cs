using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Domain.Model;
using Logic.Implementations;
using Logic.Interfaces;
using Moq;

[TestFixture]
public class PlantDataManagerTests
{
    private Mock<IPlantDataDAO> plantDataDaoMock;
    private PlantDataManagerImpl plantDataManagerImpl;
    private Mock<INotificationSender> notificationSenderMock;
    private Mock<IActionsSender> actionSenderMock;
    private INotificationSender notification;

    [SetUp]
    public void Setup()
    {
        var notificationSenderMock = new Mock<HttpClientNotificationSender>();
        var actionSenderMock = new Mock<IActionsSender>();
        plantDataDaoMock = new Mock<IPlantDataDAO>();
        plantDataManagerImpl = new PlantDataManagerImpl(plantDataDaoMock.Object, notificationSenderMock.Object, actionSenderMock.Object);
    }
    [Test]
    public async Task SaveAsync_ValidPlantData_ReturnsPlantData()
    {

        var plantDataCreationDto = new PlantDataCreationDTO {Temperature = 1, Moisture = 2, UVLight = 3, Humidity = 4, DeviceId = 1, TankLevel = 12, TimeStamp = "5/12/2023 13.44.23"};
        var expectedPlantData = new PlantData {Temperature = 1, Moisture = 2, UVLight = 3, Humidity = 4, TankLevel = 12, TimeStamp = "5/12/2023 13.44.23"};

        List<PlantDataCreationDTO> list = new List<PlantDataCreationDTO>();
        list.Add(plantDataCreationDto);

        PlantDataCreationListDTO dataList = new PlantDataCreationListDTO()
        {
            PlantDataApi = list
        };

        plantDataDaoMock.Setup(dao => dao.SaveAsync(It.IsAny<PlantDataCreationListDTO>()))
                        .ReturnsAsync(expectedPlantData);

         
        var result = await plantDataManagerImpl.SaveAsync(dataList);

         
        Assert.NotNull(result);
    }
    [Test]
    public void SaveAsync_ExceptionThrown()
    {
        var plantDataCreationDto = new PlantDataCreationDTO {Temperature = 1, Moisture = 2, UVLight = 3, Humidity = 4, DeviceId = 1, TankLevel = 12, TimeStamp = "5/12/2023 13.44.23"};

        List<PlantDataCreationDTO> list = new List<PlantDataCreationDTO>();
        list.Add(plantDataCreationDto);
        PlantDataCreationListDTO dataList = new PlantDataCreationListDTO()
        {
            PlantDataApi = list
        };
        plantDataDaoMock.Setup(x => x.SaveAsync(It.IsAny<PlantDataCreationListDTO>())).ThrowsAsync(new Exception());

        Assert.ThrowsAsync<Exception>(async () => await plantDataManagerImpl.SaveAsync(dataList));
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
        Assert.That(result.Count(), Is.EqualTo(0)); 
    }
    
    
    [Test]
    public void FilterPlantDataForLastSevenDays_WithOldPlantData()
    {
        var plantDatas = GetSamplePlantDataOld();

        var result = plantDataManagerImpl.FilterPlantDataForLastSevenDays(plantDatas);
        
        Assert.That(result, Is.Empty);
    }
    
    [Test]
    public void GroupPlantDataByDate_ShouldGroupCorrectly()
    {
        var plantDatas = GetSamplePlantData();
        
        var result = plantDataManagerImpl.GroupPlantDataByDate(plantDatas);
        
        Assert.IsNotNull(result);
        Assert.That(result.Count(), Is.EqualTo(2));  
        
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
        
        var firstGroup = result.First(); 
        var secondGroup = result.Skip(1).First(); 

        Assert.That(firstGroup.date, Is.EqualTo(new DateOnly(2023, 11, 01))); 
        Assert.That(firstGroup.avgHumidity, Is.EqualTo(50.0)); 
        Assert.That(firstGroup.avgTemperature, Is.EqualTo(25.0)); 
        Assert.That(firstGroup.avgMoisture, Is.EqualTo(30.0));
        Assert.That(firstGroup.avgUVLight, Is.EqualTo(100.0));

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
        Assert.That(result.Count, Is.EqualTo(0)); 
    }
    
    [Test]
    public async Task FetchPlantDataAsync_HumidityExceedsMaxDifference_DecreasesPercentageStatus()
    {
        var plantData = new PlantData
        {
            Humidity = 100,
            Moisture = 10,
            Temperature = 30,
            UVLight = 200,
            PlantDevice = new Device { DeviceId = 1, Plant = new Plant { PlantPreset = new PlantPreset { Humidity = 10, Moisture = 10, UVLight = 200, Temperature = 30} } },
            TankLevel = 100
        };

        plantDataDaoMock.Setup(dao => dao.FetchPlantDataAsync(It.IsAny<int>())).ReturnsAsync(new List<PlantData> { plantData });

        var result = await plantDataManagerImpl.FetchPlantDataAsync(1);

        Assert.AreEqual(75, result[0].PercentageStatus);
    }

    [Test]
    public async Task FetchPlantDataAsync_TempExceedsMaxDifference_DecreasesPercentageStatus()
    {
        var plantData = new PlantData
        {
            Humidity = 10,
            Moisture = 10,
            Temperature = 27,
            UVLight = 200,
            PlantDevice = new Device { DeviceId = 1, Plant = new Plant { PlantPreset = new PlantPreset { Humidity = 10, Moisture = 10, UVLight = 200, Temperature = 30} } },
            TankLevel = 100
        };

        plantDataDaoMock.Setup(dao => dao.FetchPlantDataAsync(It.IsAny<int>())).ReturnsAsync(new List<PlantData> { plantData });

        var result = await plantDataManagerImpl.FetchPlantDataAsync(1);

        Assert.AreEqual(75, result[0].PercentageStatus);

    }
    
    [Test]
    public async Task FetchPlantDataAsync_AllExceedMaxDifference_DecreasesPercentageStatus()
    {
        var plantData = new PlantData
        {
            Humidity = 100,
            Moisture = 100,
            Temperature = 100,
            UVLight = 300,
            PlantDevice = new Device { DeviceId = 1, Plant = new Plant { PlantPreset = new PlantPreset { Humidity = 10, Moisture = 10, UVLight = 200, Temperature = 30} } },
            TankLevel = 100
        };

        plantDataDaoMock.Setup(dao => dao.FetchPlantDataAsync(It.IsAny<int>())).ReturnsAsync(new List<PlantData> { plantData });

        var result = await plantDataManagerImpl.FetchPlantDataAsync(1);

        Assert.AreEqual(0, result[0].PercentageStatus);

    }
    
    [Test]
    public async Task FetchPlantDataAsync_AllConditionsWithinAllowedDifferences_NoChanges()
    {
       
        var plantData = new PlantData
        {
            Humidity = 10,
            Moisture = 10,
            Temperature = 30,
            UVLight = 200,
            PlantDevice = new Device { DeviceId = 1, Plant = new Plant { PlantPreset = new PlantPreset { Humidity = 10, Moisture = 10, UVLight = 200, Temperature = 30} } },
            TankLevel = 100
        };

        plantDataDaoMock.Setup(dao => dao.FetchPlantDataAsync(It.IsAny<int>())).ReturnsAsync(new List<PlantData> { plantData });
        var result = await plantDataManagerImpl.FetchPlantDataAsync(1);

        // Assert
        Assert.AreEqual(100, result[0].PercentageStatus);
        
        // Assert
        Assert.AreEqual(100, result[0].PercentageStatus);
         Assert.AreEqual(50, result[0].TankLevel);

    }
    
}



