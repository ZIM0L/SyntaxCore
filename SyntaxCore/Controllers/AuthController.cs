using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SyntaxCore.Application.Authentication.Commands;
using SyntaxCore.Entities;
using SyntaxCore.Interfaces;
using SyntaxCore.Models;
using System.Security.Claims;
using SyntaxCore.Application.Authentication.Queries;

namespace SyntaxCore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMediator _mediator;

    public AuthController(IJwtTokenService jwtTokenService, IMediator mediator)
    {
        _jwtTokenService = jwtTokenService;
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto userDto)
    {
        var request = new RegisterUserRequest(userDto.Username, userDto.Password, userDto.Email);
        var result = await _mediator.Send(request);

        if (result.)
        {
            return BadRequest("User already exists.");
        }
        return Ok(result);
    }
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserDto userDto)
     {
        var request = new LoginUserRequest(userDto.Email, userDto.Password);
        var token = _mediator.Send(request).Result;
        if (token.Equals(string.Empty))
        {
            return Unauthorized("Invalid credentials.");
        }
        return Ok(token);
     }
}
