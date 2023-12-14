
using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Logic.Implementations;
using Logic.Interfaces;
using Moq;

[TestFixture]
public class DeviceManagerImplTests
{
    private Mock<IDeviceDAO> deviceDAO;
    private IDeviceManager deviceManager;
    
    [SetUp]
    public void Setup()
    {
        deviceDAO = new Mock<IDeviceDAO>();
        deviceManager = new DeviceManagerImpl(deviceDAO.Object);
    }

    [Test]
    public async Task CreateAsync_ShouldCallDeviceDAOCreateAsync()
    {
        var deviceDAO = new Mock<IDeviceDAO>();
        var deviceManager = new DeviceManagerImpl(deviceDAO.Object);
        int newDeviceId = 1;

        await deviceManager.CreateAsync(newDeviceId);

        deviceDAO.Verify(d => d.CreateAsync(It.Is<DeviceRegistrationDTO>(dto => dto.DeviceId == newDeviceId)), Times.Once);
    }

    [Test]
    public async Task GetDeviceIdAsync_ShouldCallDeviceDAOGetDeviceIdAsync()
    {
        var deviceDAO = new Mock<IDeviceDAO>();
        var deviceManager = new DeviceManagerImpl(deviceDAO.Object);
        int deviceId = 1;

        await deviceManager.GetDeviceIdAsync(deviceId);

        deviceDAO.Verify(d => d.GetDeviceIdAsync(deviceId), Times.Once);
    }

    [Test]
    public async Task GetAllDeviceIdsAsync_ShouldCallDeviceDAOGetAllDeviceIdsAsync()
    {
        var deviceDAO = new Mock<IDeviceDAO>();
        var deviceManager = new DeviceManagerImpl(deviceDAO.Object);

        await deviceManager.GetAllDeviceIdsAsync();

        deviceDAO.Verify(d => d.GetAllDeviceIdsAsync(), Times.Once);
    }

    [Test]
    public async Task SetStatusByIdAsync_ShouldCallDeviceDAOSetStatusById()
    {
        var deviceStatusDTO = new DeviceStatusDTO
        {
            Status = false,
            DeviceId = 1
        };

        await deviceManager.SetStatusByIdAsync(deviceStatusDTO);

        deviceDAO.Verify(d => d.SetStatusById(deviceStatusDTO), Times.Once);
    }
}
