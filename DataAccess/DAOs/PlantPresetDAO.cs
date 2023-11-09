using DataAccess.DAOInterfaces;
using Domain.DTOs;
using Domain.Model;
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
               Temperature = preset.Temperature
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
        throw new NotImplementedException();
    }
}