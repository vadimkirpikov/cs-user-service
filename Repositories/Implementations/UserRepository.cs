using CsApi.Data;
using CsApi.Models.Domain;
using CsApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CsApi.Repositories.Implementations;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await context.Users.SingleOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task CreateUserAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task<User?> GetUserById(int userId)
    {
        return await context.Users.SingleOrDefaultAsync(u => u!.Id == userId);
    }

    public async Task<IEnumerable<User>> GetSubscribersAsync(int userId, int page, int pageSize)
    {
        return await context.Subscriptions
            .Where(s => s.SubscribedUserId == userId)
            .Select(s => s.Subscriber)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetSubscribedUsersAsync(int userId, int page, int pageSize)
    {
        return await context.Subscriptions
            .Where(s => s.SubscriberId == userId)
            .Select(s => s.SubscribedUser)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetUsersAsync(int page, int pageSize)
    {
        return await context.Users
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}