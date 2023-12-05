using Domain.DTOs;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackEndAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AnalyticsController : ControllerBase
{
    private IPlantDataManager plantDataManager;


    public AnalyticsController(IPlantDataManager plantDataManager)
    {
        this.plantDataManager = plantDataManager;
    }
    
    [HttpGet]
    [Route("{plantId:int}")]
    public async Task<ActionResult<List<AnalyticsDTO>>>GetAnalyticsData(int plantId)
    {
        try
        {
            List<AnalyticsDTO> analyticsData = await plantDataManager.GetPlantDataForAnalytics(plantId);
            return Ok(analyticsData);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

}