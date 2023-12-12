// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using BackEndAPI.Controllers;
// using Domain.DTOs;
// using Domain.Model;
// using Logic.Interfaces;
// using Microsoft.AspNetCore.Mvc;
// using Moq;
// using NUnit.Framework;
// using Microsoft.Extensions.Configuration;
//
// namespace YourNamespace.Tests.Controllers
// {
//     [TestFixture]
//     public class UserControllerTests
//     {
//         private Mock<IUserManager> userManagerMock;
//         private Mock<IConfiguration> configurationMock;
//         private UserController userController;
//
//         [SetUp]
//         public void Setup()
//         {
//             userManagerMock = new Mock<IUserManager>();
//             configurationMock = new Mock<IConfiguration>();
//             userController = new UserController(userManagerMock.Object, configurationMock.Object);
//         }
//
//         [Test]
//         public async Task CreateAsync_ValidUser_ReturnsCreatedResultWithUser()
//         {
//             // Arrange
//             var userCreationDto = new UserDTO { /* Set properties for valid user creation */ };
//             var newUser = new User { /* Set properties for the created user */ };
//             userManagerMock.Setup(manager => manager.CreateAsync(It.IsAny<UserDTO>())).ReturnsAsync(newUser);
//
//             // Act
//             var result = await userController.CreateAsync(userCreationDto);
//             var createdResult = result.Result as CreatedResult;
//
//             // Assert
//             Assert.IsNotNull(createdResult);
//             Assert.AreEqual(201, createdResult.StatusCode);
//             Assert.AreEqual($"/file/{newUser.UserId}", createdResult.Location);
//             Assert.AreEqual(newUser, createdResult.Value);
//         }
//
//         [Test]
//         public async Task GetAllUsersAsync_UsersExist_ReturnsOkResultWithUsers()
//         {
//             // Arrange
//             var users = new List<User> { new User { /* Set properties for user 1 */ }, new User { /* Set properties for user 2 */ } };
//             userManagerMock.Setup(manager => manager.GetAllUsersAsync()).ReturnsAsync(users);
//
//             // Act
//             var result = await userController.GetAllUsersAsync();
//             var okResult = result.Result as OkObjectResult;
//
//             // Assert
//             Assert.IsNotNull(okResult);
//             Assert.AreEqual(200, okResult.StatusCode);
//             Assert.AreEqual(users, okResult.Value);
//         }
//
//           [Test]
//         public async Task Login_ValidUser_ReturnsOkResultWithLoginResponseDto()
//         {
//             // Arrange
//             var userLoginDto = new UserDTO
//             {
//                 Username = "Anushri",
//                 Password = "Gupta"
//             };
//             var user = new User
//             {
//                 Username = "Anushri",
//                 Password = "Gupta"
//             };
//
//             userManagerMock.Setup(manager => manager.ValidateUser(It.IsAny<UserDTO>())).ReturnsAsync(user);
//             configurationMock.Setup(config => config["Jwt:Issuer"]).Returns("yourIssuer");
//             configurationMock.Setup(config => config["Jwt:Audience"]).Returns("yourAudience");
//             configurationMock.Setup(config => config["Jwt:Key"]).Returns("yourKey");
//
//             // Act
//             var result = await userController.Login(userLoginDto);
//             var okResult = result.Result as OkObjectResult;
//
//             // Assert
//             Assert.IsNotNull(okResult);
//             Assert.AreEqual(200, okResult.StatusCode);
//
//         
//             // Add more assertions based on your actual data and response structure
//         }
//
//         [Test]
//         public async Task EditAsync_ValidUser_ReturnsOkResultWithEditedUser()
//         {
//             // Arrange
//             var userToUpdateDto = new UserDTO
//             {
//                 UserId = 1,
//                 Username = "Test",
//                 Password = "Test"
//             };
//             var editedUser = new User
//             {
//                 UserId = 1,
//                 Username = "Test",
//                 Password = "Test"
//             };
//
//             userManagerMock.Setup(manager => manager.EditAsync(It.IsAny<UserDTO>())).ReturnsAsync(editedUser);
//
//             // Act
//             var result = await userController.EditAsync(userToUpdateDto);
//
//             var createdResult = result.Result;
//             Console.WriteLine(result.ToString());
//             // Assert
//             // Assert.AreEqual(200, result.Result.);
//             // Assert.AreEqual(editedUser, result.Result.Value);
//         }
//
//         [Test]
//         public async Task RemoveAsync_ValidUserId_ReturnsOkResult()
//         {
//             // Arrange
//             var userId = 1; // Set a valid user ID
//             userManagerMock.Setup(manager => manager.RemoveAsync(It.IsAny<int>()));
//
//             // Act
//             var result = await userController.RemoveAsync(userId);
//             var okResult = result as OkResult;
//
//             // Assert
//             Assert.IsNotNull(okResult);
//             Assert.AreEqual(200, okResult.StatusCode);
//         }
//     }
//
// }
