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
    public async Task<ActionResult<PlantData>> CreateAsync([FromBody] PlantDataCreationListDTO plantData)
    {
        Console.WriteLine($"Recieved data in this format: {plantData.ToString()}");
        try
        {
            await plantDataManager.SaveAsync(plantData);
            //await plantDataManager.CheckDataWithPlantPreset(newPlantData);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    

    [HttpGet]
    [Route("fetchPlantData/{userId:int}")]
    public async Task<ActionResult<List<PlantData>>> FetchPlantData([FromRoute] int userId)
    {
        try
        {
            List<PlantData> plantData = await plantDataManager.FetchPlantDataAsync(userId);
            return Ok(plantData);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

}