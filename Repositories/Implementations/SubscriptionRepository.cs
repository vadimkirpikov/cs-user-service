using CsApi.Data;
using CsApi.Models.Domain;
using CsApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CsApi.Repositories.Implementations;

public class SubscriptionRepository(ApplicationDbContext context) : ISubscriptionRepository
{
    public async Task<Subscription?> GetSubscriptionAsync(int subscriberId, int subscribedUserId)
    {
        return await context.Subscriptions
            .FirstOrDefaultAsync(s => s.SubscriberId == subscriberId && s.SubscribedUserId == subscribedUserId);
    }
    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        await context.Subscriptions.AddAsync(subscription);
        await context.SaveChangesAsync();
    }

    public async Task RemoveSubscriptionAsync(Subscription subscription)
    {
        context.Subscriptions.Remove(subscription);
        await context.SaveChangesAsync();
    }
}
