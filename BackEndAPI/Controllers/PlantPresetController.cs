using Domain.DTOs;
using Domain.Model;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackEndAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class PlantPresetController : ControllerBase 
{
    
    private IPlantPresetManager plantPresetManager;


    public PlantPresetController(IPlantPresetManager plantPresetManager)
    {
        this.plantPresetManager = plantPresetManager;
    }
    
    
    [HttpPost]
    [Route("createPlantPreset")]
    public async Task<ActionResult<PlantPreset>> CreateAsync(PlantPresetCreationDTO plantPresetCreationDto)
    {
        try
        {
            PlantPreset newPlant = await plantPresetManager.CreateAsync(plantPresetCreationDto);
            return Created($"/file/{newPlant.PresetId}", newPlant);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }

    }
    
    [HttpGet]
    [Route("getPresetById/{presetId:int}")]
    public async Task<ActionResult<PlantPreset>> GetPresetById(int presetId)
    {

        try
        {
            PlantPreset plantPreset = await plantPresetManager.GetByIdAsync(presetId);
            return Ok(plantPreset);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlantPreset>>> GetAllPlantPresetsAsync()
    {
        try
        {
            IEnumerable<PlantPreset> presets = await plantPresetManager.GetAllPlantPresetsAsync();
            return Ok(presets.ToList());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet]
    [Route("getPresetsByUser/{userId:int}")]
    public async Task<ActionResult<IEnumerable<PlantPreset>>> GetPresetsByUserId(int userId)
    {

        try
        {
            IEnumerable<PlantPreset> plantPresets = await plantPresetManager.GetPresetsByUserIdAsync(userId);
            return Ok(plantPresets.ToList());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

}