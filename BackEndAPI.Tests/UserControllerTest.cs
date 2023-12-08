using System.Net;
using System.Text;
using System.Web.Http;
using Domain.DTOs;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using NUnit.Framework;

namespace IntegrationTests
{

    [TestFixture]
    public class UserControllerIntegrationTests
    {
        private TestServer _server;
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
        public async Task CreateAsync_ShouldReachController()
        {
           //Arrange
            var userCreationDto = new UserDTO
            {
                Username = "Anushri",
                Password = "Gupta"
            };

            var content = new StringContent(JsonConvert.SerializeObject(userCreationDto), Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("User/createUser", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Test]
        public async Task GetAllUsersAsync_ShouldReachController()
        {
            // Act
            var response = await _client.GetAsync("User");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task Login_ShouldReachController()
        {
            // Arrange
            var userLogin = new UserDTO
            {
                Username = "Anushri",
                Password = "Gupta"
            };

            var content = new StringContent(JsonConvert.SerializeObject(userLogin), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("User/login", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task EditAsync_ShouldReachController()
        {
            
            // Arrange
            var userToUpdate = new UserDTO
            {
                UserId = 1,
                Username = "Anushri",
                Password = "Gupta"
            };

            var content = new StringContent(JsonConvert.SerializeObject(userToUpdate), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PatchAsync("User", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Dispose of resources
            _client.Dispose();
        }
    }
}
