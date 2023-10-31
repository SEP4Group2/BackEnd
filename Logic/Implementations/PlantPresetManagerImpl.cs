﻿using DataAccess.DAOInterfaces;
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
}