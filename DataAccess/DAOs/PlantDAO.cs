using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Domain.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccess.DAOs;

public class PlantDAO : IPlantDAO
{
    private readonly AppContext _appContext;

    public PlantDAO(AppContext appContext)
    {
        _appContext = appContext;
    }

    public async Task<Plant> CreateAsync(PlantCreationDTO plantCreationDto)
    {
        try
        {
            var plant = new Plant()
            {
                Location = plantCreationDto.Location,
                Type = plantCreationDto.Type,
                // PlantPreset = plantCreationDto.PlantPreset, 
                Name = plantCreationDto.Name
            };
            EntityEntry<Plant> newPlant = await _appContext.Plants.AddAsync(plant);
            await _appContext.SaveChangesAsync();
            return newPlant.Entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Plant> GetAsync(int id)
    {
        Plant? plant = null;
        try
        {
            plant = await _appContext.Plants.FindAsync(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return plant;
    }
}