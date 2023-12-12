using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Domain.Model;
using Logic.Implementations;
using Logic.Interfaces;
using Moq;


[TestFixture]
public class PlantManagerTests
{
    private Mock<IPlantDAO> mockPlantDao;
    private IPlantManager plantManager;
    
    [SetUp]
    public void Setup()
    {
        // Mock PlantDataDao
        mockPlantDao = new Mock<IPlantDAO>();
        // Create an instance of the class under test, passing the mock dependency
        plantManager = new PlantManagerImpl(mockPlantDao.Object);
    }
    [Test]
        public async Task CreateAsync_ValidPlantCreationDTO_ReturnsPlant()
        {
            // Arrange
            mockPlantDao.Setup(dao => dao.CreateAsync(It.IsAny<PlantCreationDTO>()))
                .ReturnsAsync(new Plant { Name = "TestPlant",
                    Location = "TestLocation",
                    PlantPreset = new Mock<PlantPreset>().Object });


            var validPlantCreationDto = new PlantCreationDTO
            {
                Name = "TestPlant",
                Location = "TestLocation",
                PlantPresetId = 1
            };

            // Act
            var result = await plantManager.CreateAsync(validPlantCreationDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Plant>(result);
        }

        [Test]
        public void CreateAsync_InvalidPlantPresetId_ThrowsArgumentException()
        {
           
       
            var invalidPlantCreationDto = new PlantCreationDTO
            {
                Name = "TestPlant",
                Location = "TestLocation",
                PlantPresetId = 0 // Invalid PlantPresetId
            };

            Assert.ThrowsAsync<ArgumentException>(() => plantManager.CreateAsync(invalidPlantCreationDto));
        }

        [Test]
        public async Task GetAllPlantsAsync_ReturnsListOfGetAllPlantsDTO()
        {
            mockPlantDao.Setup(dao => dao.GetAllPlantsAsync(1)).ReturnsAsync(new List<GetAllPlantsDTO>
            {
                new GetAllPlantsDTO (2, "Tina1", "garden", new Mock<PlantPreset>().Object, 1,1),
                new GetAllPlantsDTO (3, "Tina2", "garden", new Mock<PlantPreset>().Object,2,2),
                new GetAllPlantsDTO (4, "Tina3", "garden", new Mock<PlantPreset>().Object,3,3),

            });

            
            var result = await plantManager.GetAllPlantsAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<GetAllPlantsDTO>>(result);
        }

        [Test]
        public async Task RemoveAsync_ValidId_CallsPlantDaoRemoveAsync()
        {
           

            var validId = 1;

            // Act
            await plantManager.RemoveAsync(validId);

            // Assert
            mockPlantDao.Verify(dao => dao.RemoveAsync(validId), Times.Once);
        }

        [Test]
        public async Task GetAsync_ValidId_ReturnsPlant()
        {
            mockPlantDao.Setup(dao => dao.GetAsync(It.IsAny<int>())).ReturnsAsync(new Plant { /* Initialize properties */ });

            var validId = 1;

            // Act
            var result = await plantManager.GetAsync(validId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Plant>(result);
        }

        [Test]
        public async Task CreateAsync_WithInvalidPlantPresetId_ThrowsArgumentException()
        {
            var plantCreationDto = new PlantCreationDTO
            {
                PlantPresetId = -1, // Invalid ID
            };
          
            Assert.ThrowsAsync<ArgumentException>(async () => await plantManager.CreateAsync(plantCreationDto));
        }
        [Test]
        public async Task EditAsync_WhenCalled_ShouldReturnEditedPlant()
        {
            

            var user1 = new User
            {
                UserId = 1, 
                Username = "testUser1", 
                Password = "testPassword"
            };
        
            var plantPreset = new PlantPreset
            {
                PresetId = 1,
                UserId = 1, // Assuming a valid user ID for testing
                Name = "TestPreset",
                Humidity = 50,
                UVLight = 500,
                Moisture = 30,
                Temperature = 25,
            };
            var device = new Device 
            { 
                DeviceId = 1,
                Status = true,
            };
           
            var plantCreationDto = new Plant{
                Location = "TestLocation",
                PlantPreset = plantPreset, // Assuming a valid preset ID for testing
                User = user1, // Assuming a valid user ID for testing
                Name = "TestPlant",
                IconId = 1
            };

       
            var plantToUpdate = new EditPlantDTO()
            {
                PlantId = plantCreationDto.PlantId,
                Name = "MyPlant",
                Location = "Room"
            };
         

            mockPlantDao.Setup(dao => dao.EditAsync(plantToUpdate)).ReturnsAsync(plantCreationDto);

            // Act
            var result = await plantManager.EditAsync(plantToUpdate);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(plantCreationDto, result);
        }
}