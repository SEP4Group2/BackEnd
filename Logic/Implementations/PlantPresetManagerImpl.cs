using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Domain.Model;
using Logic.Interfaces;

namespace Logic.Implementations;

public class PlantPresetManagerImpl : IPlantPresetManager
{
    private IPlantPresetDAO plantPresetDao;

    public PlantPresetManagerImpl(IPlantPresetDAO plantPresetDao)
    {
        this.plantPresetDao = plantPresetDao; 
    }

    public async Task<PlantPreset> CreateAsync(PlantPresetCreationDTO plantCreationDto)
    {
        return await plantPresetDao.CreateAsync(plantCreationDto);
    }

    public async Task<PlantPreset> GetByIdAsync(int presetId)
    {
        return await plantPresetDao.GetAsync(presetId);
    }

    public async Task<IEnumerable<PlantPreset>> GetAllPresetsAsync(int userId)
    {
        return await plantPresetDao.GetAllPresetsAsync(userId);
    }
}