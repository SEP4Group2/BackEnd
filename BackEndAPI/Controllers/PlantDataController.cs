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
    public async Task<ActionResult<PlantData>> CreateAsync([FromBody] PlantData plantData)
    {
        try
        {
            PlantData newPlantData = await plantDataManager.SaveAsync(plantData);
            return Created($"/plant/{newPlantData.TimeStamp}", newPlantData);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}