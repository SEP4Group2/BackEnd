using DataAccess.DAOs;
using Domain.Model;

namespace Tests.DataAccess;
using System.Threading.Tasks;
using Domain.DTOs;
using NUnit.Framework;

[TestFixture]
public class PlantPresetDAOTests : DatabaseTestFixture
{
    private PlantPresetDAO _plantPresetDao;

    [SetUp]
    public void SetUp()
    {
        _plantPresetDao = new PlantPresetDAO(Context);
    }

    [Test]
    public async Task CreateAsync_ShouldCreatePlantPreset()
    {
        //Clear database
        
        ClearDatabase();
            
        await Context.SaveChangesAsync();
        
        // Arrange
        var user1 = new User{UserId = 1, Username = "testUser1", Password = "testPassword"};
        Context.Users.Add(user1);
        
        var plantPresetCreationDto = new PlantPresetCreationDTO
        {
            UserId = 1, // Assuming a valid user ID for testing
            Name = "TestPreset",
            Humidity = 50,
            UVLight = 500,
            Moisture = 30,
            Temperature = 25
        };

        // Act
        var createdPreset = await _plantPresetDao.CreateAsync(plantPresetCreationDto);

        // Assert
        Assert.IsNotNull(createdPreset);
        Assert.AreEqual(plantPresetCreationDto.Name, createdPreset.Name);
        Assert.AreEqual(plantPresetCreationDto.Humidity, createdPreset.Humidity);
        Assert.AreEqual(plantPresetCreationDto.UVLight, createdPreset.UVLight);
        Assert.AreEqual(plantPresetCreationDto.Moisture, createdPreset.Moisture);
        Assert.AreEqual(plantPresetCreationDto.Temperature, createdPreset.Temperature);

    }

    [Test]
    public async Task GetAsync_ShouldReturnPlantPreset()
    {
        //Clear database
        
        ClearDatabase();
            
        await Context.SaveChangesAsync();
        // Arrange
        var presetId = 1; // Assuming a valid preset ID for testing
        var user1 = new User
        {
            UserId = 1, 
            Username = "testUser1", 
            Password = "testPassword"
        };
        
        var plantPreset = new PlantPreset
        {
            PresetId = presetId,
            UserId = 1, // Assuming a valid user ID for testing
            Name = "TestPreset",
            Humidity = 50,
            UVLight = 500,
            Moisture = 30,
            Temperature = 25,
        };
        Context.Users.Add(user1);
        Context.Presets.Add(plantPreset);
        await Context.SaveChangesAsync();
        

        // Act
        var result = await _plantPresetDao.GetAsync(presetId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(presetId, result.PresetId);

    }

    [Test]
    public async Task GetAllPresetsAsync_ShouldReturnListOfPresets()
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
        
        var plantPreset1 = new PlantPreset
        {
            UserId = 1, // Assuming a valid user ID for testing
            Name = "TestPreset",
            Humidity = 50,
            UVLight = 500,
            Moisture = 30,
            Temperature = 25
        };
        
        var plantPreset2 = new PlantPreset
        {
            UserId = 1, // Assuming a valid user ID for testing
            Name = "TestPreset",
            Humidity = 50,
            UVLight = 500,
            Moisture = 30,
            Temperature = 25
        };
        
        var plantPreset3 = new PlantPreset
        {
            UserId = 1, // Assuming a valid user ID for testing
            Name = "TestPreset",
            Humidity = 50,
            UVLight = 500,
            Moisture = 30,
            Temperature = 25
        };

        Context.Users.Add(user1);
        Context.Presets.Add(plantPreset1);
        Context.Presets.Add(plantPreset2);
        Context.Presets.Add(plantPreset3);

        await Context.SaveChangesAsync();


        // Act
        var presets = await _plantPresetDao.GetAllPresetsAsync(userId);

        // Assert
        Assert.IsNotNull(presets);
        Assert.IsInstanceOf<List<PlantPreset>>(presets);
        
    }
}