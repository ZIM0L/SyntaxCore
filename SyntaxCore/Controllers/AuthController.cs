using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyntaxCore.Application.Authentication.Commands;
using SyntaxCore.Interfaces;
using SyntaxCore.Models;
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
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
    {
        var request = new RegisterUserRequest(registerUserDto.Username, registerUserDto.Password, registerUserDto.Email);
        var result = await _mediator.Send(request);

        if (result.Equals(String.Empty))
        {
            return BadRequest("User already exists.");
        }
        return Ok(result);
    }
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginUserDto loginUserDto)
     {
        var request = new LoginUserRequest(loginUserDto.Email, loginUserDto.Password);
        var token = _mediator.Send(request).Result;
        if (token.Equals(string.Empty))
        {
            return Unauthorized("Invalid credentials.");
        }
        return Ok(token);
     }
}
