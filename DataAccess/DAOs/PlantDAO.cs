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
            Device? existingDevice = await _appContext.Devices.FindAsync(plantCreationDto.DeviceId);
            if (existingDevice == null) throw new Exception("Device does not exist in the database");
            
            
                var plant = new Plant()
            {
                Location = plantCreationDto.Location,
                PlantPreset = existingPreset,
                Name = plantCreationDto.Name,
                User = existingUser
            };
            EntityEntry<Plant> newPlant = await _appContext.Plants.AddAsync(plant);
            await _appContext.SaveChangesAsync();
            
            //add Plant object to the already existing Device 
            existingDevice.Plant = plant;
            _appContext.Devices.Update(existingDevice);
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

    public async Task<List<GetAllPlantsDTO>> GetAllPlantsAsync(int userId)
    {
        List<Plant> plants = await _appContext.Plants.Where(p => p.User.UserId == userId).Include(p => p.PlantPreset).ToListAsync();
        List<GetAllPlantsDTO> listDtos = new List<GetAllPlantsDTO>();
        foreach (var plant in plants)
        {
            Device device = _appContext.Devices.FirstOrDefault(d => d.Plant.PlantId == plant.PlantId);
            GetAllPlantsDTO dto = new GetAllPlantsDTO(plant.PlantId, plant.Name, plant.Location, plant.PlantPreset,
                device.DeviceId);
            listDtos.Add(dto);
        }
        return listDtos;
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

    public async Task<Plant> EditAsync(EditPlantDTO editPlantDto)
    {
        Plant plantToBeEdited = _appContext.Plants.First(p => p.PlantId == editPlantDto.PlantId);
        if (editPlantDto.Location != null) plantToBeEdited.Location = editPlantDto.Location;
        if (editPlantDto.Name != null) plantToBeEdited.Name = editPlantDto.Name;
        try
        {
            _appContext.Plants.Update(plantToBeEdited);
            await _appContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception(ex.Message);
        }
        return plantToBeEdited;
    }
}