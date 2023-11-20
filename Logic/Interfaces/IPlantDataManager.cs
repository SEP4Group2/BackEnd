﻿using DataAccess.DAOInterfaces;
using Domain.Model;

namespace Logic.Interfaces;

public interface IPlantDataManager
{
    
   
    Task<PlantData> SaveAsync(PlantData plantData);
    Task<List<PlantData>> GetAllByPlantIdAsync(int id);
}