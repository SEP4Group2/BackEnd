using Domain.DTOs;
using Domain.Model;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackEndAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class PlantDataController : ControllerBase
{
    private IPlantDataManager plantDataManager;


    public PlantDataController(IPlantDataManager plantDataManager)
    {
        this.plantDataManager = plantDataManager;
    }



    [HttpPost]
    [Route("savePlantData")]
    public async Task<ActionResult<PlantData>> CreateAsync([FromBody] PlantDataCreationDTO plantData)
    {
        try
        {
            PlantData newPlantData = await plantDataManager.SaveAsync(plantData);
            await plantDataManager.CheckDataWithPlantPreset(newPlantData);
            return Created($"/plant/{newPlantData.TimeStamp}", newPlantData);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    [Route("fetchPlantDataByPlantId/{plantId:int}")]
    public async Task<ActionResult<List<PlantData>>> GetByPlantIdAsync([FromRoute] int plantId)
    {
        // id to be used once we associate iot device to a particular plant - as of now, not implemented
        try
        {
            List<PlantData> plantDatas = await plantDataManager.GetAllByPlantIdAsync(plantId);
            return Ok(plantDatas);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }


    }

    [HttpGet]
    [Route("fetchPlantData/{userId:int}")]
    public async Task<ActionResult<List<PlantData>>> GetAsync([FromRoute] int userId)
    {
        try
        {
            List<PlantData> plantData = await plantDataManager.GetAllByUserIdAsync(userId);
            return Ok(plantData);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

}