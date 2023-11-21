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
    [Route("{presetId:int}")]
    public async Task<ActionResult<PlantPreset>> GetPlant(int presetId)
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

}