using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Domain.Model;
using Logic.Interfaces;

namespace Logic.Implementations;

public class PlantManagerImpl : IPlantManager
{

    private IPlantDAO plantDao;

    public PlantManagerImpl(IPlantDAO plantDao)
    {
        this.plantDao = plantDao;
    }

    public async Task<Plant> CreateAsync(PlantCreationDTO plantCreationDto)
    {
        return await plantDao.CreateAsync(plantCreationDto);
    }

    public async Task<Plant> GetAsync(int id)
    {
        return await plantDao.GetAsync(id);
    }
}