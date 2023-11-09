﻿using DataAccess.DAOInterfaces;
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
        try
        {
            return await plantDao.GetAsync(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}