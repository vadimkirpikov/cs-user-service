using System.Security.Claims;
using CsApi.Filters;
using CsApi.Models.Dto;
using CsApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CsApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/subscriptions")]
public class SubscriptionController(ISubscriptionService subscriptionService) : ControllerBase
{
    [HttpGet("{subscriberId:int}/{subscribedUserId:int}")]
    public async Task<IActionResult> GetSubscription([FromRoute] int subscriberId, [FromRoute] int subscribedUserId)
    {
        var subscription = await subscriptionService.GetSubscriptionAsync(subscriberId, subscribedUserId);
        if (subscription == null)
        {
            return NotFound("Подписка не найдена");
        }
        return Ok(subscription);
    }
    
    [HttpPost("subscribe/{id:int}")]
    [CustomExceptionFilter]
    public async Task<IActionResult> AddSubscription([FromRoute] int id)
    {
        var subscriberId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await subscriptionService.AddSubscriptionAsync(new SubscriptionDto { SubscriberId = subscriberId, SubscribedUserId = id });
        return Ok("Подписка успешно добавлена");
    }

    [HttpDelete("unsubscribe/{id:int}")]
    [CustomExceptionFilter]
    public async Task<IActionResult> RemoveSubscription([FromRoute] int id)
    {
        var subscriberId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await subscriptionService.RemoveSubscriptionAsync(subscriberId, id);
        return Ok("Подписка успешно удалена");
    }
}