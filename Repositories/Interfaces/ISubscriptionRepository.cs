using CsApi.Models.Domain;

namespace CsApi.Repositories.Interfaces;

public interface ISubscriptionRepository
{
    Task<Subscription?> GetSubscriptionAsync(int subscriberId, int subscribedUserId);
    Task AddSubscriptionAsync(Subscription subscription);
    Task RemoveSubscriptionAsync(Subscription subscription);
}
