using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyntaxCore.Application.Authentication.Commands.RefreshToken;
using SyntaxCore.Application.Authentication.Commands.Register;
using SyntaxCore.Application.Authentication.Queries.Login;
using SyntaxCore.Interfaces;
using SyntaxCore.Models.UserRelated;

namespace SyntaxCore.Controllers;

[Route("api/auth")]
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
    [AllowAnonymous]
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
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginUserDto loginUserDto)
     {
        var request = new LoginUserRequest(loginUserDto.Email, loginUserDto.Password);
        var token = _mediator.Send(request).Result;
        if (token.AccessToken.Equals(string.Empty))
        {
            return Unauthorized("Invalid credentials.");
        }
        return Ok(token);
     }
    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> TestAuth([FromBody] RefreshTokenDto refreshTokenDto)
    {
        var request = new RefreshTokenRequest(refreshTokenDto.RefreshToken);
        var result = await _mediator.Send(request);
        return Ok(result);
    }

}
