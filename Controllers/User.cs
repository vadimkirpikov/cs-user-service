﻿using CsApi.Filters;
using CsApi.Models.Domain;
using CsApi.Models.Dto;
using CsApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CsApi.Controllers;


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

    [Authorize]
    [HttpPut("{userId:int}")]
    [CustomExceptionFilter]
    public async Task<IActionResult> UpdateUser([FromRoute] int userId, [FromBody] UpdateUserDto updateUserDto)
    {
        await userService.UpdateUserAsync(userId, updateUserDto);
        return Ok("Информация о пользователе обновлена");
    }

    [Authorize]
    [HttpGet("{userId:int}/subscribers/page/{page:int}/page-size/{pageSize:int}")]
    [CustomExceptionFilter]
    public async Task<IActionResult> GetSubscribers([FromRoute] int userId,[FromRoute] int page, [FromRoute] int pageSize)
    {
        var subscribers = await userService.GetSubscribersAsync(userId, page, pageSize);
        return Ok(subscribers);
    }
    
    [Authorize]
    [HttpGet("{userId:int}/subscribed/page/{page:int}/page-size/{pageSize:int}")]
    [CustomExceptionFilter]
    public async Task<IActionResult> GetSubscribedUsers([FromRoute] int userId,[FromRoute] int page, [FromRoute] int pageSize)
    {
        var subscribedUsers = await userService.GetSubscribedUsersAsync(userId, page, pageSize);
        return Ok(subscribedUsers);
    }

    [Authorize]
    [HttpGet("full/{id:int}")]
    [CustomExceptionFilter]
    public async Task<IActionResult> GetUser([FromRoute] int id)
    {
        var result = await userService.GetUserToSendAsync(id);
        return Ok(result);
    }
    
    [HttpGet("notify/{id:int}")]
    [CustomExceptionFilter]
    public async Task<IActionResult> GetUserToNotify([FromRoute] int id)
    {
        var result = await userService.GetUserToNotifyAsync(id);
        return Ok(result);
    }
}