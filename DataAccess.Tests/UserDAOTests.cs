using DataAccess.DAOs;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

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
        [Test]
        public async Task EditAsync_ShouldEditUser()
        {
            // Arrange
            ClearDatabase();

            // Create a user to edit
            var userCreationDto = new UserDTO
            {
                Username = "TestUser",
                Password = "TestPassword"
            };
            var createdUser = await _userDao.CreateAsync(userCreationDto);

            // Modify user data
            var userDtoToUpdate = new UserDTO
            {
                UserId = createdUser.UserId,
                Username = "UpdatedUsername",
                Password = "UpdatedPassword"
            };

            // Act
            var updatedUser = await _userDao.EditAsync(userDtoToUpdate);

            // Assert
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(userDtoToUpdate.UserId, updatedUser.UserId);
            Assert.AreEqual(userDtoToUpdate.Username, updatedUser.Username);
            Assert.AreEqual(userDtoToUpdate.Password, updatedUser.Password);
        }

        [Test]
        public async Task EditAsync_AlreadyExistingUsername_ShouldThrowException()
        {
            // Arrange
            ClearDatabase();

            var user1 = new User
            {
                UserId = 1,
                Username = "TestUser1",
                Password = "TestPassword"
            };
            var user2 = new User
            {
                UserId = 2,
                Username = "TestUser2",
                Password = "TestPassword"
            };

            Context.Users.Add(user1);
            Context.Users.Add(user2);
            await Context.SaveChangesAsync();

            var userDtoToUpdate = new UserDTO
            {
                UserId = 1, // Invalid user ID
                Username = "TestUser2",
                Password = "UpdatedPassword"
            };

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _userDao.EditAsync(userDtoToUpdate));
        }

        [Test]
        public async Task RemoveAsync_ShouldRemoveUserAndAssociatedData()
        {
            // Arrange
            ClearDatabase();
            
            var user = new User
            {
                UserId = 1,
                Username = "TestUser",
                Password = "TestPassword"
            };
            
            var plantPreset = new PlantPreset
                        {
                            PresetId = 1,
                            UserId = 1,
                            Name = "TestPreset",
                            Humidity = 50,
                            UVLight = 800,
                            Moisture = 30,
                            Temperature = 23
                        };
            var plant = new Plant
                        {
                            PlantId = 1,
                            User = user,
                            Location = "testLocation",
                            Name = "testPreset",
                            PlantPreset = plantPreset,
                            IconId = 1
                        };
            var device = new Device
            {
                DeviceId = 1,
                Status = true,
                Plant = plant
            };

            Context.Users.Add(user);
            Context.Presets.Add(plantPreset);
            Context.Plants.Add(plant);
            Context.Devices.Add(device);

            await Context.SaveChangesAsync();

            // Act
            await _userDao.RemoveAsync(user.UserId);

            // Assert throwing an exception when there are no users int he database
            Assert.ThrowsAsync<Exception>(async () => await _userDao.GetAllUsersAsync());

            // Ensure associated plants and presets are removed
            var plants = await Context.Plants.ToListAsync();
            var presets = await Context.Presets.ToListAsync();
            Assert.IsEmpty(plants);
            Assert.IsEmpty(presets);
        }

        [Test]
        public async Task RemoveAsync_InvalidUserId_ShouldThrowException()
        {
            // Arrange
            ClearDatabase();

            // Attempt to remove a non-existent user
            var invalidUserId = 1; 

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _userDao.RemoveAsync(invalidUserId));
        }
    }

