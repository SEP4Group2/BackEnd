using Domain.DTOs;
using Domain.Model;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackEndAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private IUserManager userManager;

    public UserController(IUserManager userManager)
    {
        this.userManager = userManager;
    }
    
    [HttpPost]
    [Route("createUser")]
    public async Task<ActionResult<User>> CreateAsync(UserCreationDto userCreationDto)
    {
        try
        {
            User newUser = await userManager.CreateAsync(userCreationDto);
            return Created($"/file/{newUser.UserId}", newUser);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }

    }

}