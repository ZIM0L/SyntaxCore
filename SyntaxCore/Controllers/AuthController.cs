using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SyntaxCore.Entities;
using SyntaxCore.Interfaces;
using SyntaxCore.Models;

namespace SyntaxCore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _jwtTokenService;
    private static readonly User _user = new();
    public AuthController(IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] UserDto userDto)
    {
        var hashedPassword = new PasswordHasher<User>()
            .HashPassword(_user, userDto.Password);
        
        _user.Username = userDto.Username;
        _user.PasswordHash = hashedPassword;

        return Ok(_user);
    }
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserDto userDto)
     {
         var result = new PasswordHasher<User>().VerifyHashedPassword(_user, _user.PasswordHash, userDto.Password);
         if (result == PasswordVerificationResult.Failed)
         {
             return Unauthorized();
         }

         var token = _jwtTokenService.GenerateToken(_user);

         return Ok(token);
     }
}
