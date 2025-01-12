using CsApi.Models.Domain;

namespace CsApi.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(int id);
    Task CreateUserAsync(User user);
    Task UpdateUserAsync(User user);
    
    Task<IEnumerable<User>> GetSubscribersAsync(int userId, int page, int pageSize);
    Task<IEnumerable<User>> GetSubscribedUsersAsync(int userId, int page, int pageSize);
    Task<IEnumerable<User>> GetUsersAsync(int page, int pageSize);
}
