
using CsApi.Models.Domain;
using CsApi.Models.Dto;
using CsApi.Repositories.Interfaces;
using CsApi.Services.Implementations;
using Moq;
using Xunit;
namespace CsApi.Tests;


public class SubscriptionServiceTests
{
    private readonly Mock<ISubscriptionRepository> _mockRepository;
    private readonly SubscriptionService _service;

    public SubscriptionServiceTests()
    {
        _mockRepository = new Mock<ISubscriptionRepository>();
        _service = new SubscriptionService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetSubscriptionAsync_ShouldReturnSubscription_WhenExists()
    {
        var subscriberId = 1;
        var subscribedUserId = 2;
        var expectedSubscription = new Subscription
        {
            SubscriberId = subscriberId,
            SubscribedUserId = subscribedUserId
        };
        _mockRepository
            .Setup(repo => repo.GetSubscriptionAsync(subscriberId, subscribedUserId))
            .ReturnsAsync(expectedSubscription);
        
        var result = await _service.GetSubscriptionAsync(subscriberId, subscribedUserId);
        
        Assert.NotNull(result);
        Assert.Equal(expectedSubscription.SubscriberId, result.SubscriberId);
        Assert.Equal(expectedSubscription.SubscribedUserId, result.SubscribedUserId);
    }

    [Fact]
    public async Task AddSubscriptionAsync_ShouldThrowArgumentException_WhenSubscriptionExists()
    {
        var subscriptionDto = new SubscriptionDto { SubscriberId = 1, SubscribedUserId = 2 };
        var existingSubscription = new Subscription
        {
            SubscriberId = subscriptionDto.SubscriberId,
            SubscribedUserId = subscriptionDto.SubscribedUserId
        };
        _mockRepository
            .Setup(repo => repo.GetSubscriptionAsync(subscriptionDto.SubscriberId, subscriptionDto.SubscribedUserId))
            .ReturnsAsync(existingSubscription);
        
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _service.AddSubscriptionAsync(subscriptionDto));
        Assert.Equal($"Пользователь с id {subscriptionDto.SubscriberId} уже подписан на пользователя с id {subscriptionDto.SubscribedUserId}", exception.Message);
    }

    [Fact]
    public async Task AddSubscriptionAsync_ShouldAddSubscription_WhenNotExists()
    {
        var subscriptionDto = new SubscriptionDto { SubscriberId = 1, SubscribedUserId = 2 };
        _mockRepository
            .Setup(repo => repo.GetSubscriptionAsync(subscriptionDto.SubscriberId, subscriptionDto.SubscribedUserId))
            .ReturnsAsync((Subscription?)null);
        
        await _service.AddSubscriptionAsync(subscriptionDto);
        
        _mockRepository.Verify(repo => repo.AddSubscriptionAsync(It.Is<Subscription>(
            s => s.SubscriberId == subscriptionDto.SubscriberId &&
                 s.SubscribedUserId == subscriptionDto.SubscribedUserId)), Times.Once);
    }

    [Fact]
    public async Task RemoveSubscriptionAsync_ShouldThrowArgumentException_WhenSubscriptionDoesNotExist()
    {
        var subscriberId = 1;
        var subscribedUserId = 2;
        _mockRepository
            .Setup(repo => repo.GetSubscriptionAsync(subscriberId, subscribedUserId))
            .ReturnsAsync((Subscription?)null);
        
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _service.RemoveSubscriptionAsync(subscriberId, subscribedUserId));
        Assert.Equal($"Пользователь с id {subscriberId} не подписан на пользователя с id {subscribedUserId}", exception.Message);
    }

    [Fact]
    public async Task RemoveSubscriptionAsync_ShouldRemoveSubscription_WhenExists()
    {
        var subscriberId = 1;
        var subscribedUserId = 2;
        var existingSubscription = new Subscription
        {
            SubscriberId = subscriberId,
            SubscribedUserId = subscribedUserId
        };
        _mockRepository
            .Setup(repo => repo.GetSubscriptionAsync(subscriberId, subscribedUserId))
            .ReturnsAsync(existingSubscription);
        
        await _service.RemoveSubscriptionAsync(subscriberId, subscribedUserId);
        
        _mockRepository.Verify(repo => repo.RemoveSubscriptionAsync(existingSubscription), Times.Once);
    }
}
