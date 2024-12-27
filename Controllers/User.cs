using CsApi.Filters;
using CsApi.Models.Domain;
using CsApi.Models.Dto;
using CsApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CsApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/users")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    [CustomExceptionFilter]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto)
    {
        await userService.RegisterUserAsync(registerDto);
        return Ok("Пользователь успешно зарегистрирован");
    }

    [HttpPost("login")]
    [CustomExceptionFilter]
    public async Task<IActionResult> LoginUser([FromBody] LoginDto loginDto)
    {
        var token = await userService.LoginUserAsync(loginDto);
        return Ok(new { Token = token });
    }

    [HttpPut("{userId:int}")]
    [CustomExceptionFilter]
    public async Task<IActionResult> UpdateUser([FromRoute] int userId, [FromBody] UpdateUserDto updateUserDto)
    {
        await userService.UpdateUserAsync(userId, updateUserDto);
        return Ok("Информация о пользователе обновлена");
    }

    [HttpGet("{userId:int}/subscribers")]
    [CustomExceptionFilter]
    public async Task<IActionResult> GetSubscribers([FromRoute] int userId)
    {
        var subscribers = await userService.GetSubscribersAsync(userId);
        return Ok(subscribers);
    }

    [HttpGet("{userId:int}/subscribed")]
    [CustomExceptionFilter]
    public async Task<IActionResult> GetSubscribedUsers([FromRoute] int userId)
    {
        var subscribedUsers = await userService.GetSubscribedUsersAsync(userId);
        return Ok(subscribedUsers);
    }
}