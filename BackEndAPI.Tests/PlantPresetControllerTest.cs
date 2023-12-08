using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Model;
using Newtonsoft.Json;
using NUnit.Framework;
namespace BackEndAPI.Tests

{
   [TestFixture]
    public class PlantPresetControllerIntegrationTests : DatabaseTestFixture
    {
      
        private HttpClient _client;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Arrange
            _client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:5000")
            };
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

            var content = new StringContent(JsonConvert.SerializeObject(plantPresetCreationDto), Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/PlantPreset/createPlantPreset", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode,
                $"Expected HTTP 201 Created, but received {response.StatusCode}");

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

            // Act
            var response = await _client.GetAsync($"/PlantPreset/getPreset/{plantPreset.PresetId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task GetPresetsByUserId_ShouldReturnOkStatus()
        {
            // Arrange
            ClearDatabase();
            // Add a sample user and plant preset to the database
            var user = new User
            {
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

            // Act
            var response = await _client.GetAsync($"/PlantPreset/getAllPresets/{user.UserId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
      
        
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _client.Dispose();
        }
    }

}