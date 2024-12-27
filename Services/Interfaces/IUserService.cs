using CsApi.Models.Domain;
using CsApi.Models.Dto;

namespace CsApi.Services.Interfaces;

public interface IUserService
{
    Task RegisterUserAsync(RegisterDto registerDto);
    Task<string> LoginUserAsync(LoginDto loginDto);
    Task UpdateUserAsync(int userId, UpdateUserDto updateUserDto);
    Task<IEnumerable<User>> GetSubscribersAsync(int userId);
    Task<IEnumerable<User>> GetSubscribedUsersAsync(int userId);
}
