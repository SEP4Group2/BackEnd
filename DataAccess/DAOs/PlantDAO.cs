using System.Linq.Expressions;
using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
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
            PlantPreset? existing = await _appContext.Presets.FindAsync(plantCreationDto.PlantPresetId);
            if (existing == null) throw new Exception("Preset not found");

                var plant = new Plant()
            {
                Location = plantCreationDto.Location,
                PlantPreset = existing,
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
        Plant? plant = await _appContext.Plants.FindAsync(id);
            if (plant == null)
            {
                throw new Exception("Plant not found");
            }
            
        return plant;
    }

    public async Task<List<GetAllPlantsDTO>> GetAllPlantsAsync()
    {
        return await _appContext.Plants
            .Select(p => new GetAllPlantsDTO (
                p.PlantId, p.Name, p.Location, p.PlantPreset))
            .ToListAsync();
    }
}