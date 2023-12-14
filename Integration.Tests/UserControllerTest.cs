using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackEndAPI.Controllers;
using DataAccess.DAOInterfaces;
using DataAccess.DAOs;
using Domain.DTOs;
using Domain.Model;
using Logic.Implementations;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Engine;

[TestFixture]
    public class UserControllerTests : DatabaseTestFixture
    {
        private IUserManager userManager;
        private IUserDAO userDao;
        private UserController controller;
        private Mock<IConfiguration> configMock;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            userDao = new UserDAO(Context);
            userManager = new UserManagerImpl(userDao);
            configMock = new Mock<IConfiguration>();
            controller = new UserController(userManager, configMock.Object);

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
            var result = await controller.CreateAsync(userCreationDto);

            var createdResult = result.Result;
            Assert.That(createdResult, Is.TypeOf<CreatedResult>());
        }

      
        [Test]
        public async Task Login_WithValidUser_ReturnsOkResultWithToken()
        {
            ClearDatabase();
            // Arrange
            var user = new User
            {
                UserId = 1,
                Username = "testUser",
                Password = "testPassword"
            };

            Context.Users.Add(user);
            await Context.SaveChangesAsync();
            var userLoginDto = new UserDTO
            {
                UserId = 1,
                Username = "testUser",
                Password = "testPassword"
            };

            // Act
            var result = await userManager.ValidateUser(userLoginDto);

            // 
            Assert.NotNull(result);
            
        }


        [Test]
        public async Task GetAllUsersAsync_ShouldReachController()
        {

            ClearDatabase();

            var user1 = new User()
            {
                Username = "Anushri",
                Password = "Gupta"
            };
            // Act
            var user2 = new User()
            {
                Username = "Anushri2",
                Password = "Hehehehheh"
            };
            var user3 = new User()
            {
                Username = "Anushri3",
                Password = "Gupta2"
            };

            Context.Users.Add(user1);
            Context.Users.Add(user2);
            Context.Users.Add(user3);
            await Context.SaveChangesAsync();

            var result = await controller.GetAllUsersAsync();
            var createdResult = result.Result;
            Assert.That(createdResult, Is.TypeOf<OkObjectResult>());

            // Assert

        }

        [Test]
        public async Task GetAllUsersAsync_WithNoUsers_ShouldReturnNotFound()
        {
            // Arrange
            ClearDatabase();

            var result = await controller.GetAllUsersAsync();

            var createdResult = result.Result;
            // Assert
            Assert.IsInstanceOf<ObjectResult>(createdResult);
            var statusCodeResult = createdResult as ObjectResult;
            Assert.AreEqual(500, statusCodeResult.StatusCode);
        }

 
        [Test]
        public async Task EditAsync_ShouldReachController()
        {
            var user = new User()
            {
                Username = "Test",
                Password = "Gupta"
            };

            Context.Users.Add(user);
            await Context.SaveChangesAsync();
            
            // Arrange
            var userToUpdate = new UserDTO
            {
                Username = "Anushri",
                Password = "Gupta"
            };

            
            var result = await controller.EditAsync(userToUpdate);
            var createdResult = result.Result;
            Console.WriteLine($"Result Content: {createdResult?.ToString()}");

            Assert.That(createdResult,Is.TypeOf<ObjectResult>()) ;
            
        }

        
        [Test]
        public async Task DeleteUser_ShouldReturnOk()
        {
            // Arrange
            ClearDatabase();
            var user1 = new User
            {
                UserId = 1, 
                Username = "testUser1", 
                Password = "testPassword"
            };
        
        
            Context.Users.Add(user1);
           
            await Context.SaveChangesAsync();

          

            var result = await controller.RemoveAsync(user1.UserId);
            Console.WriteLine($"Actual Result Type: {result?.GetType()}");

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task RemoveAsync_InvalidUserId_ShouldReturnInternalServerError()
        {
            // Arrange
            ClearDatabase();

            // Act
            var result = await controller.RemoveAsync(6); 

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var statusCodeResult = result as ObjectResult;
            Assert.AreEqual(500, statusCodeResult.StatusCode);
        }

     
}

