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
    
    [HttpPost]
    [CustomExceptionFilter]
    public async Task<IActionResult> AddSubscription([FromBody] SubscriptionDto subscriptionDto)
    {
        await subscriptionService.AddSubscriptionAsync(subscriptionDto);
        return Ok("Подписка успешно добавлена");
    }

    [HttpDelete("{subscriberId:int}/{subscribedUserId:int}")]
    [CustomExceptionFilter]
    public async Task<IActionResult> RemoveSubscription([FromRoute] int subscriberId, [FromRoute] int subscribedUserId)
    {
        await subscriptionService.RemoveSubscriptionAsync(subscriberId, subscribedUserId);
        return Ok("Подписка успешно удалена");
    }
}