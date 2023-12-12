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
    
    [SetUp]
    public void Setup()
    {
        var notificationSenderMock = new Mock<INotificationSender>();
        var actionSenderMock = new Mock<IActionsSender>();
        // Mock PlantDataDao
        plantDataDaoMock = new Mock<IPlantDataDAO>();
        // Create an instance of the class under test, passing the mock dependency
        plantDataManagerImpl = new PlantDataManagerImpl(plantDataDaoMock.Object, notificationSenderMock.Object, actionSenderMock.Object);
    }
    [Test]
    public async Task SaveAsync_ValidPlantData_ReturnsPlantData()
    {
        // Arrange

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

    // [Test]
    // public async Task CheckDataWithPlantPreset_OutsideOptimalRange_SendsNotification()
    // {
    //     // Arrange
    //        // Arrange
    //              var user1 = new User
    //              {
    //                  UserId = 1, 
    //                  Username = "testUser1", 
    //                  Password = "testPassword"
    //              };
    //          
    //              var plantPreset = new PlantPreset
    //              {
    //                  PresetId = 1,
    //                  UserId = 1, // Assuming a valid user ID for testing
    //                  Name = "TestPreset",
    //                  Humidity = 50,
    //                  UVLight = 500,
    //                  Moisture = 30,
    //                  Temperature = 25,
    //              };
    //              
    //              var plant = new Plant
    //              {
    //                  PlantId = 1,
    //                  User = user1,
    //                  Location = "testLocation",
    //                  Name = "testPreset",
    //                  PlantPreset = plantPreset,
    //                  IconId = 1
    //              };
    //              var device = new Device 
    //              { 
    //                  DeviceId = 1,
    //                  Status = true,
    //                  Plant = plant
    //              };
    //            
    //              var plantData1 = new PlantData()
    //              {
    //                  PlantDevice = device, // Assuming a valid device ID for testing
    //                  Humidity = 50,
    //                  Temperature = 25,
    //                  Moisture = 300,
    //                  UVLight = 500,
    //                  TimeStamp = "11/02/2023,11:11:03",
    //                  TankLevel = 75
    //              };
    //              var plantData2 = new PlantData()
    //              {
    //
    //                  PlantDevice = device, // Assuming a valid device ID for testing
    //                  Humidity = 50,
    //                  Temperature = 25,
    //                  Moisture = 300,
    //                  UVLight = 501,
    //                  TimeStamp = "11/02/2023,11:11:05",
    //                  TankLevel = 75
    //              };
    //
    //              var dto1 = new PlantDataCreationDTO(){
    //                  
    //                  DeviceId = plantData1.PlantDevice.DeviceId, // Assuming a valid device ID for testing
    //                  Humidity = plantData1.Humidity,
    //                  Temperature = plantData1.Temperature,
    //                  Moisture = plantData1.Moisture,
    //                  UVLight = plantData1.UVLight,
    //                  TimeStamp = plantData1.TimeStamp,
    //                  TankLevel = plantData1.TankLevel
    //              };
    //              
    //              var dto2= new PlantDataCreationDTO(){
    //                  
    //                  DeviceId = plantData2.PlantDevice.DeviceId, // Assuming a valid device ID for testing
    //                  Humidity = plantData2.Humidity,
    //                  Temperature = plantData2.Temperature,
    //                  Moisture = plantData2.Moisture,
    //                  UVLight = plantData2.UVLight,
    //                  TimeStamp = plantData2.TimeStamp,
    //                  TankLevel = plantData2.TankLevel
    //              };
    //              
    //     
    //     var plantData = new PlantDataCreationDTO()
    //     {
    //         Humidity = 15,
    //         Temperature = 25,
    //         UVLight = 10,
    //         Moisture = 20,
    //         DeviceId = 1,
    //         TankLevel = 10
    //     };
    //
    //     List<PlantDataCreationDTO> list = new List<PlantDataCreationDTO>();
    //     list.Add(plantData);
    //
    //     PlantDataCreationListDTO dataList = new PlantDataCreationListDTO()
    //     {
    //         PlantDataApi = list
    //     };
    //     plantDataDaoMock.Setup(dao => dao.SaveAsync(It.IsAny<PlantDataCreationListDTO>()))
    //         .ReturnsAsync(plantData);
    //
    //     // Act
    //     await plantDataManagerImpl.CheckDataWithPlantPreset(plantData);
    //
    //     // Assert
    //     notificationSenderMock.Verify(sender => sender.SendNotification(It.IsAny<NotificationRequestDTO>()), Times.Exactly(4));
    //     // Add more assertions based on the expected behavior of the method
    // }
    //
    // [Test]
    // public async Task FetchPlantDataAsync_ValidUserId_ReturnsPlantDataList()
    // {
    //     // Arrange
    //     var actionMock = new Mock<IActionsSender>();
    //     var userDaoMock = new Mock<IUserDAO>();
    //     var plantDataManager = new PlantDataManagerImpl(plantDataDaoMock.Object, notificationSenderMock.Object, actionMock.Object);
    //
    //     var user = new User()
    //     {
    //         Username = "Test",
    //         Password = "Test"
    //     };
    //
    //     userDaoMock.Setup(dao => dao.CreateAsync(It.IsAny<UserDTO>())).ReturnsAsync(user);
    //     var expectedPlantDataList = new List<PlantData> {new PlantData()
    //     {
    //         Humidity = 1, Temperature = 1, Moisture = 1, TankLevel = 1, TimeStamp = "5/12/2023 13.44.22", UVLight = 1
    //     },new PlantData()
    //         {
    //         Humidity = 1, Temperature = 2, Moisture = 3, TankLevel = 4, TimeStamp = "5/12/2023 13.44.22", UVLight = 5
    //     }};
    //
    //     plantDataDaoMock.Setup(dao => dao.FetchPlantDataAsync(It.IsAny<int>()))
    //                     .ReturnsAsync(expectedPlantDataList);
    //
    //     // Act
    //     var result = await plantDataManager.FetchPlantDataAsync(user.UserId);
    //
    //     // Assert
    //     Assert.NotNull(result);
    // }
    //
    
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
