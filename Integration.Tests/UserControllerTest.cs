using System.Net;
using BackEndAPI.Controllers;
using BackEndAPI.Tests;
using DataAccess.DAOInterfaces;
using DataAccess.DAOs;
using Domain.DTOs;
using Domain.Model;
using Logic.Implementations;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Moq;
using NUnit.Framework;


[TestFixture]
    public class UserControllerTests : DatabaseTestFixture
    {
        private IUserManager userManager;
        private IUserDAO userDao;
        private UserController controller;
        private readonly IConfiguration config;
    
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            userDao = new UserDAO(Context);
            userManager = new UserManagerImpl(userDao);
            controller = new UserController(userManager, config);
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
        public async Task EditAsync_ShouldReachController()
        {
            
            
            // Arrange
            var userToUpdate = new UserDTO
            {
                UserId = 1,
                Username = "Anushri",
                Password = "Gupta"
            };

            
            var result = await controller.EditAsync(userToUpdate);
            var createdResult = result.Result;
            Console.WriteLine($"Result Content: {createdResult?.ToString()}");

            Assert.IsInstanceOf<ActionResult<User>>(result);
            
        }

        
        //Rainy scenarios
      
}

