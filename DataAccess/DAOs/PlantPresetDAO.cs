using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccess.DAOs;

public class PlantPresetDAO : IPlantPresetDAO
{

    private readonly AppContext _appContext;

    public PlantPresetDAO(AppContext _appContext)
    {
        this._appContext = _appContext;
    }
    public async Task<PlantPreset> CreateAsync(PlantPresetCreationDTO preset)
    {
        try
        {

            var plantPreset = new PlantPreset()
            {
                Name = preset.Name,
                Humidity = preset.Humidity,
               UVLight = preset.UVLight,
               Moisture = preset.Moisture,
               Temperature = preset.Temperature,
               UserId = preset.UserId
            };
            
           EntityEntry<PlantPreset> newPreset = await _appContext.Presets.AddAsync(plantPreset);
            await _appContext.SaveChangesAsync();
            return newPreset.Entity;

        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PlantPreset> GetAsync(int id)
    {
        PlantPreset? plantPreset = await _appContext.Presets.FindAsync(id);
        if (plantPreset == null)
        {
            throw new Exception("Plant Preset not found");
        }
            
        return plantPreset;
    }
    
    public async Task<List<PlantPreset>> GetAllPlantPresentsAsync()
    {
        try
        {
            return await _appContext.Presets.ToListAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
    
    public async Task<List<PlantPreset>> GetPresetsByUserIdAsync(int userId)
    {
        try
        {
            return await _appContext.Presets.Where(p=>p.UserId == userId).ToListAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}