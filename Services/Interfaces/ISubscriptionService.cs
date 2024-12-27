using CsApi.Models.Domain;
using CsApi.Models.Dto;

namespace CsApi.Services.Interfaces;

public interface ISubscriptionService
{
    Task<Subscription?> GetSubscriptionAsync(int subscriberId, int subscribedUserId);
    Task AddSubscriptionAsync(SubscriptionDto subscriptionDto);
    Task RemoveSubscriptionAsync(int subscriberId, int subscribedUserId);
}