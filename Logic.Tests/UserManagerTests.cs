
using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Domain.Model;
using Logic.Implementations;
using Logic.Interfaces;
using Moq;

[TestFixture]
public class UserManagerImplTests
{
    private Mock<IUserDAO> userDaoMock;
    private IUserManager userManagerImpl;
    
    [SetUp]
    public void Setup()
    {
        userDaoMock = new Mock<IUserDAO>();
        userManagerImpl = new UserManagerImpl(userDaoMock.Object);
    }
    
    
    [Test]
    public async Task CreateUserAsync_ValidUser_ReturnsUser()
    {
        var userDto = new UserDTO {Username = "Anushri", Password = "Gupta"};
        var mockUserDao = new Mock<IUserDAO>();
        mockUserDao.Setup(dao => dao.CreateAsync(It.IsAny<UserDTO>()))
                   .ReturnsAsync(new User {Username = "Anushri", Password = "Gupta"});
        var userManager = new UserManagerImpl(mockUserDao.Object);
        var result = await userManager.CreateAsync(userDto);
        Assert.NotNull(result);
    }

    [Test]
    public async Task GetAllUsersAsync_ReturnsUsersList()
    {
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

        var result = await userManager.GetAllUsersAsync();

        Assert.NotNull(result);
    }

    [Test]
    public async Task ValidateUser_ValidCredentials_ReturnsUser()
    {
        var userDto = new UserDTO { Username = "Anushri1", Password = "Gupta"};
        userDaoMock.Setup(dao => dao.GetAllUsersAsync())
                   .ReturnsAsync(new List<User> { new User()
                   {
                       Username = "Anushri1", Password = "Gupta"
                   },
                       new User()
                       {
                           Username = "Username1", Password = "Password"
                       }
                   });

        var result = await userManagerImpl.ValidateUser(userDto);

        Assert.NotNull(result);
    }

    [Test]
    public async Task ValidateUser_UserNotFound_ThrowsException()
    {
        var userDto = new UserDTO { Username = "Wrong", Password = "User"};
        userDaoMock.Setup(dao => dao.GetAllUsersAsync())
                   .ReturnsAsync(new List<User> {new User()
                   {
                       Username = "Test2", Password = "Test1"
                   } ,
                       new User()
                       {
                           Username = "Test3", Password = "Test11"
                       }
                   });

        Assert.ThrowsAsync<Exception>(async () => await userManagerImpl.ValidateUser(userDto));
    }

    [Test]
    public async Task ValidateUser_PasswordMismatch_ThrowsException()
    {
        var userDto = new UserDTO {Username = "User", Password = "Wrong"};
        userDaoMock.Setup(dao => dao.GetAllUsersAsync())
                   .ReturnsAsync(new List<User> {new User()
                   {
                       Username = "User", Password = "Right"
                   } ,
                       new User()
                       {
                           Username = "Test", Password = "Test"
                       }
                   });

        Assert.ThrowsAsync<Exception>(async () => await userManagerImpl.ValidateUser(userDto));
    }
    
    [Test]
    public async Task RemoveAsync_ValidId_CallsUserDaoRemoveAsync()
    {

        var validId = 1;

        await userManagerImpl.RemoveAsync(validId);

        userDaoMock.Verify(dao => dao.RemoveAsync(validId), Times.Once);
    }
    [Test]
    public async Task EditAsync_WhenCalled_ShouldReturnEditedUser()
    {

        var userDto = new UserDTO
        {
           Username = "Test",
           Password = "Test"
        };

        var editedUser = new User
        {
            UserId = userDto.UserId,
            Username = "EditedTest",
            Password = "Test"
        };

        userDaoMock.Setup(dao => dao.EditAsync(userDto)).ReturnsAsync(editedUser);

        var result = await userManagerImpl.EditAsync(userDto);

        Assert.IsNotNull(result);
        Assert.AreEqual(editedUser, result);
    }
}
