using DataAccess.DAOs;
using Domain.Model;

namespace Tests.DataAccess;
using System.Threading.Tasks;
using Domain.DTOs;
using NUnit.Framework;

    [TestFixture]
    public class UserDAOTests : DatabaseTestFixture
    {
        private UserDAO _userDao;

        [SetUp]
        public void SetUp()
        {
            _userDao = new UserDAO(Context);
        }

        [Test]
        public async Task CreateAsync_ShouldCreateUser()
        {
            //Clear database
            
            ClearDatabase();
            
            await Context.SaveChangesAsync();
            // Arrange
            var userCreationDto = new UserDTO
            {
                Username = "TestUser",
                Password = "TestPassword"
            };

            // Act
            var createdUser = await _userDao.CreateAsync(userCreationDto);

            // Assert
            Assert.IsNotNull(createdUser);
            Assert.AreEqual(userCreationDto.Username, createdUser.Username);
            Assert.AreEqual(userCreationDto.Password, createdUser.Password);
            
        }

        [Test]
        public async Task GetAllUsersAsync_ShouldReturnListOfUsers()
        {
            //Clear database
            
            ClearDatabase();
            
            await Context.SaveChangesAsync();
            //Arrange
            var user1 = new User{UserId = 1, Username = "testUser1", Password = "testPassword"};
            var user2 = new User{UserId = 2, Username = "testUser2", Password = "testPassword"};
            var user3 = new User{UserId = 3, Username = "testUser3", Password = "testPassword"};

            Context.Users.Add(user1);
            Context.Users.Add(user2);
            Context.Users.Add(user3);

            await Context.SaveChangesAsync();


            
            // Act
            var users = await _userDao.GetAllUsersAsync();

            // Assert
            Assert.IsNotNull(users);
            Assert.IsInstanceOf<IEnumerable<User>>(users);
            
        }
    }

