using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Domain.Model;
using Logic.Implementations;
using Logic.Interfaces;
using Moq;
using NUnit.Framework;

[TestFixture]
public class UserManagerImplTests
{
    private Mock<IUserDAO> userDaoMock;
    private IUserManager userManagerImpl;
    
    [SetUp]
    public void Setup()
    {
        // Mock PlantDataDao
        userDaoMock = new Mock<IUserDAO>();
        // Create an instance of the class under test, passing the mock dependency
        userManagerImpl = new UserManagerImpl(userDaoMock.Object);
    }
    
    
    [Test]
    public async Task CreateUserAsync_ValidUser_ReturnsUser()
    {
        // Arrange
        var userDto = new UserDTO {Username = "Anushri", Password = "Gupta"};
        var mockUserDao = new Mock<IUserDAO>();
        mockUserDao.Setup(dao => dao.CreateAsync(It.IsAny<UserDTO>()))
                   .ReturnsAsync(new User {Username = "Anushri", Password = "Gupta"});
        var userManager = new UserManagerImpl(mockUserDao.Object);

        // Act
        var result = await userManager.CreateAsync(userDto);

        // Assert
        Assert.NotNull(result);
    }

    [Test]
    public async Task GetAllUsersAsync_ReturnsUsersList()
    {
        // Arrange
        var mockUserDao = new Mock<IUserDAO>();
        mockUserDao.Setup(dao => dao.GetAllUsersAsync())
                   .ReturnsAsync(new List<User> { new User()
                   {
                       Password = "Password", Username = "Username"
                   },
                       new User()
                       {
                           Password = "Gupta", Username = "Anushri"
                       }
                   });
        var userManager = new UserManagerImpl(mockUserDao.Object);

        // Act
        var result = await userManager.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
    }

    [Test]
    public async Task ValidateUser_ValidCredentials_ReturnsUser()
    {
        // Arrange
        var userDto = new UserDTO { Username = "Anushri1", Password = "Guptaa"};
        var mockUserDao = new Mock<IUserDAO>();
        mockUserDao.Setup(dao => dao.GetAllUsersAsync())
                   .ReturnsAsync(new List<User> { new User()
                   {
                       Username = "Anushri1", Password = "Guptaa"
                   },
                       new User()
                       {
                           Username = "Username1", Password = "Passowrd"
                       }
                   });
        var userManager = new UserManagerImpl(mockUserDao.Object);

        // Act
        var result = await userManager.ValidateUser(userDto);

        // Assert
        Assert.NotNull(result);
        // Add more assertions based on your requirements
    }

    [Test]
    public async Task ValidateUser_UserNotFound_ThrowsException()
    {
        // Arrange
        var userDto = new UserDTO { Username = "Wrong", Password = "User"};
        var mockUserDao = new Mock<IUserDAO>();
        mockUserDao.Setup(dao => dao.GetAllUsersAsync())
                   .ReturnsAsync(new List<User> {new User()
                   {
                       Username = "Anushri2", Password = "Gupta1"
                   } ,
                       new User()
                       {
                           Username = "Ansuhri3", Password = "Gupta11"
                       }
                   });
        var userManager = new UserManagerImpl(mockUserDao.Object);

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await userManager.ValidateUser(userDto));
    }

    [Test]
    public async Task ValidateUser_PasswordMismatch_ThrowsException()
    {
        // Arrange
        var userDto = new UserDTO {Username = "User", Password = "Wrong"};
        var mockUserDao = new Mock<IUserDAO>();
        mockUserDao.Setup(dao => dao.GetAllUsersAsync())
                   .ReturnsAsync(new List<User> {new User()
                   {
                       Username = "User", Password = "Right"
                   } ,
                       new User()
                       {
                           Username = "Anushri4", Password = "Guptaaaa"
                       }
                   });
        var userManager = new UserManagerImpl(mockUserDao.Object);

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await userManager.ValidateUser(userDto));
    }
    
    [Test]
    public async Task RemoveAsync_ValidId_CallsUserDaoRemoveAsync()
    {
        // Arrange
        

        var validId = 1;

        // Act
        await userManagerImpl.RemoveAsync(validId);

        // Assert
        userDaoMock.Verify(dao => dao.RemoveAsync(validId), Times.Once);
    }
}
