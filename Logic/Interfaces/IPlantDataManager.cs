﻿using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Domain.Model;

namespace Logic.Interfaces;

public interface IPlantDataManager
{
    
   
    Task<PlantData> SaveAsync(PlantDataCreationDTO plantData);
    Task<List<PlantData>> FetchPlantDataAsync(int userId);
    Task CheckDataWithPlantPreset(PlantData plantData);
}