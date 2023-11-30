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
            PlantPreset? existingPreset = await _appContext.Presets.FindAsync(plantCreationDto.PlantPresetId);
            if (existingPreset == null) throw new Exception("Preset not found");
            User? existingUser = await _appContext.Users.FindAsync(plantCreationDto.UserId);
            if (existingUser == null) throw new Exception("User does not exist in the database");
            
            
                var plant = new Plant()
            {
                Location = plantCreationDto.Location,
                PlantPreset = existingPreset,
                Name = plantCreationDto.Name,
                User = existingUser
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
        Plant? plant = await Task.FromResult(_appContext.Plants.Include(p => p.PlantPreset).FirstOrDefault(p=> p.PlantId == id));
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

    public async Task RemoveAsync(int id)
    {
        Plant? plant = await _appContext.Plants.FindAsync(id);
        if (plant == null)
        {
            throw new Exception("Plant not found");
        }

        _appContext.Remove(plant);
        await _appContext.SaveChangesAsync();
    }
}