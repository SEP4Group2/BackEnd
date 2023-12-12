using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Domain.Model;
using Logic.Implementations;
using Moq;

namespace Logic.Tests;

[TestFixture]
public class PlantManagerTests
{
    [Test]
        public async Task CreateAsync_ValidPlantCreationDTO_ReturnsPlant()
        {
            // Arrange
            var mockPlantDao = new Mock<IPlantDAO>();
            mockPlantDao.Setup(dao => dao.CreateAsync(It.IsAny<PlantCreationDTO>()))
                .ReturnsAsync(new Plant { Name = "TestPlant",
                    Location = "TestLocation",
                    PlantPreset = new Mock<PlantPreset>().Object });

            var plantManager = new PlantManagerImpl(mockPlantDao.Object);

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
            // Arrange
            var mockPlantDao = new Mock<IPlantDAO>();
            var plantManager = new PlantManagerImpl(mockPlantDao.Object);

            var invalidPlantCreationDto = new PlantCreationDTO
            {
                Name = "TestPlant",
                Location = "TestLocation",
                PlantPresetId = 0 // Invalid PlantPresetId
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => plantManager.CreateAsync(invalidPlantCreationDto));
        }

        [Test]
        public async Task GetAllPlantsAsync_ReturnsListOfGetAllPlantsDTO()
        {
            // Arrange
            var mockPlantDao = new Mock<IPlantDAO>();
            mockPlantDao.Setup(dao => dao.GetAllPlantsAsync(1)).ReturnsAsync(new List<GetAllPlantsDTO>
            {
                new GetAllPlantsDTO (2, "Tina1", "garden", new Mock<PlantPreset>().Object, 1,1),
                new GetAllPlantsDTO (3, "Tina2", "garden", new Mock<PlantPreset>().Object,2,2),
                new GetAllPlantsDTO (4, "Tina3", "garden", new Mock<PlantPreset>().Object,3,3),

            });

            var plantManager = new PlantManagerImpl(mockPlantDao.Object);
            
            var result = await plantManager.GetAllPlantsAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<GetAllPlantsDTO>>(result);
        }

        [Test]
        public async Task RemoveAsync_ValidId_CallsPlantDaoRemoveAsync()
        {
            // Arrange
            var mockPlantDao = new Mock<IPlantDAO>();
            var plantManager = new PlantManagerImpl(mockPlantDao.Object);

            var validId = 1;

            // Act
            await plantManager.RemoveAsync(validId);

            // Assert
            mockPlantDao.Verify(dao => dao.RemoveAsync(validId), Times.Once);
        }

        [Test]
        public async Task GetAsync_ValidId_ReturnsPlant()
        {
            // Arrange
            var mockPlantDao = new Mock<IPlantDAO>();
            mockPlantDao.Setup(dao => dao.GetAsync(It.IsAny<int>())).ReturnsAsync(new Plant { /* Initialize properties */ });

            var plantManager = new PlantManagerImpl(mockPlantDao.Object);
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
            // Arrange
            var plantCreationDto = new PlantCreationDTO
            {
                PlantPresetId = -1, // Invalid ID
            };
            var mockPlantDao = new Mock<IPlantDAO>();
            var plantManager = new PlantManagerImpl(mockPlantDao.Object);
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await plantManager.CreateAsync(plantCreationDto));
        }
}