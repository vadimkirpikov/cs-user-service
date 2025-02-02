﻿using CsApi.Models.Domain;
using CsApi.Models.Dto;
using CsApi.Repositories.Interfaces;
using CsApi.Services.Interfaces;

namespace CsApi.Services.Implementations;

public class SubscriptionService(ISubscriptionRepository subscriptionRepository) : ISubscriptionService
{
    public async Task<Subscription?> GetSubscriptionAsync(int subscriberId, int subscribedUserId)
    {
        return await subscriptionRepository.GetSubscriptionAsync(subscriberId, subscribedUserId);
    }

    public async Task AddSubscriptionAsync(SubscriptionDto subscriptionDto)
    {
        var existingSubscription = await subscriptionRepository.GetSubscriptionAsync(subscriptionDto.SubscriberId, subscriptionDto.SubscribedUserId);
        if (subscriptionDto.SubscribedUserId == subscriptionDto.SubscriberId)
        {
            throw new ArgumentException("User cannot subscribe to himself");
        }
        if (existingSubscription != null)
        {
            throw new ArgumentException($"User with id {subscriptionDto.SubscriberId} already subscribed to user with id {subscriptionDto.SubscribedUserId}");
        }
        var subscription = new Subscription
        {
            SubscriberId = subscriptionDto.SubscriberId,
            SubscribedUserId = subscriptionDto.SubscribedUserId
        };

        await subscriptionRepository.AddSubscriptionAsync(subscription);
    }

    public async Task RemoveSubscriptionAsync(int subscriberId, int subscribedUserId)
    {
        var subscription = await subscriptionRepository.GetSubscriptionAsync(subscriberId, subscribedUserId);
        if (subscription == null)
        {
            throw new ArgumentException($"User with id {subscriberId} didn't subscribe to user with id {subscribedUserId}");
        }
        await subscriptionRepository.RemoveSubscriptionAsync(subscription);
    }
}