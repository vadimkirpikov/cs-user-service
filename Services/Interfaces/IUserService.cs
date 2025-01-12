using CsApi.Models.Domain;
using CsApi.Models.Dto;

namespace CsApi.Services.Interfaces;

public interface IUserService
{
    Task<UserToNotifyDto> GetUserToNotifyAsync(int id);
    Task<SentUserDto> GetUserToSendAsync(int id);
    Task RegisterUserAsync(RegisterDto registerDto);
    Task<string> LoginUserAsync(LoginDto loginDto);
    Task UpdateUserAsync(int userId, UpdateUserDto updateUserDto);
    Task<IEnumerable<SentUserDto>> GetSubscribersAsync(int userId, int page, int pageSize);
    Task<IEnumerable<SentUserDto>> GetSubscribedUsersAsync(int userId, int page, int pageSize);
    Task<IEnumerable<UserToNotifyDto>> GetUsersAsync(int page, int pageSize);
}
