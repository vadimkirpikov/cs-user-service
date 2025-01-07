using AutoMapper;
using CsApi.Models.Domain;
using CsApi.Models.Dto;
using CsApi.Repositories.Interfaces;
using CsApi.Services.Interfaces;
using CsApi.Utils.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CsApi.Services.Implementations;

public class UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtUtils jwtUtils, IMapper mapper)
    : IUserService
{
    private async Task<User> TryGetExistingUser(int id)
    {
        var user = await userRepository.GetUserById(id);
        if (user == null)
        {
            throw new ArgumentException($"User with id: {id} was not found");
        }
        return user;
    }
    public async Task<UserToNotifyDto> GetUserToNotifyAsync(int id)
    {
        var user = await TryGetExistingUser(id);
        var userToNotifyDto = mapper.Map<UserToNotifyDto>(user);
        return userToNotifyDto;
    }

    public async Task<SentUserDto> GetUserToSendAsync(int id)
    {
        var user = await TryGetExistingUser(id);
        var userToSendDto = mapper.Map<SentUserDto>(user);
        return userToSendDto;
    }

    public async Task RegisterUserAsync(RegisterDto registerDto)
    {
        var existingUser = await userRepository.GetUserByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Пользователь с таким email уже существует");
        }

        var passwordHash = passwordHasher.HashPassword(registerDto.Password);

        var newUser = new User
        {
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            Name = registerDto.Name,
            BirthDate = registerDto.BirthDate,
            Location = registerDto.Location,
            Bio = registerDto.Bio,
            IsActive = registerDto.IsActive,
            DeviceToken = registerDto.DeviceToken,
        };
        await userRepository.CreateUserAsync(newUser);
    }

    public async Task<string> LoginUserAsync(LoginDto loginDto)
    {
        var user = await userRepository.GetUserByEmailAsync(loginDto.Email);
        if (user == null || !passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash!))
        {
            throw new UnauthorizedAccessException("Неверный email или пароль");
        }
        return jwtUtils.GenerateJwtToken(user);
    }

    public async Task UpdateUserAsync(int userId, UpdateUserDto updateUserDto)
    {
        var user = await TryGetExistingUser(userId);
        
        user.Name = updateUserDto.Name;
        user.Bio = updateUserDto.Bio;
        user.BirthDate = updateUserDto.BirthDate;
        user.Location = updateUserDto.Location;

        await userRepository.UpdateUserAsync(user);
    }

    public async Task<IEnumerable<SentUserDto>> GetSubscribersAsync(int userId, int page, int pageSize)
    {
        var users = await userRepository.GetSubscribersAsync(userId, page, pageSize);
        return mapper.Map<IEnumerable<SentUserDto>>(users);
    }

    public async Task<IEnumerable<SentUserDto>> GetSubscribedUsersAsync(int userId, int page, int pageSize)
    {
        var users = await userRepository.GetSubscribedUsersAsync(userId, page, pageSize);
        return mapper.Map<IEnumerable<SentUserDto>>(users);
    }
}
