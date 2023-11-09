using System.Linq.Expressions;
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
            PlantPreset? existing = await _appContext.Presets.FindAsync(plantCreationDto.PlantPresetId);
            if (existing == null) throw new Exception("Preset not found");

                var plant = new Plant()
            {
                Location = plantCreationDto.Location,
                Type = plantCreationDto.Type,
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

    public Task<Plant> GetAsync(int id)
    {
        throw new NotImplementedException();
    }
}