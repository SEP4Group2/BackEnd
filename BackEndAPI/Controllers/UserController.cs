using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.DTOs;
using Domain.Model;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BackEndAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private IUserManager userManager;
    private readonly IConfiguration config;


    public UserController(IUserManager userManager, IConfiguration config)
    {
        this.userManager = userManager;
        this.config = config;
    }
    
    [HttpPost]
    [Route("createUser")]
    public async Task<ActionResult<User>> CreateAsync(UserDTO userCreationDto)
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
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsersAsync()
    {
        try
        {
            IEnumerable<User> users = await userManager.GetAllUsersAsync();

            if (users == null || !users.Any())
            {
                return NotFound("No users found");
            }

            return Ok(users.ToList());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost, Route("login")]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody]UserDTO userLogin)
    {
        try
        {
            User user = await userManager.ValidateUser(userLogin);
            string token = GenerateJwt(user);
            LoginResponseDTO responseDto = new LoginResponseDTO()
            {
                Token = token,
                User = new UserDTO()
                {
                    Password = user.Password,
                    Username = user.Username,
                    UserId = user.UserId
                }
            };
            return Ok(responseDto);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
   
    private List<Claim> GenerateClaims(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, config["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("DisplayName", user.Username),
        };
        return claims.ToList();
    }
    
    private string GenerateJwt(User user)
    {
        List<Claim> claims = GenerateClaims(user);
    
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        SigningCredentials signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
    
        JwtHeader header = new JwtHeader(signIn);
    
        JwtPayload payload = new JwtPayload(
            config["Jwt:Issuer"],
            config["Jwt:Audience"],
            claims, 
            null,
            DateTime.UtcNow.AddMinutes(60));
    
        JwtSecurityToken token = new JwtSecurityToken(header, payload);
    
        string serializedToken = new JwtSecurityTokenHandler().WriteToken(token);
        return serializedToken;
    }

    [HttpPatch]
    public async Task<ActionResult<User>> EditAsync([FromBody] UserDTO userToUpdate)
    {
        try
        {
           return await userManager.EditAsync(userToUpdate);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete]
    [Route("{userId:int}")]
    public async Task<ActionResult> RemoveAsync(int userId)
    {
        try
        {
            await userManager.RemoveAsync(userId);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}