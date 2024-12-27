using CsApi.Models.Domain;
using CsApi.Models.Dto;
using CsApi.Repositories.Interfaces;
using CsApi.Services.Interfaces;
using CsApi.Utils.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CsApi.Services.Implementations;

public class UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtUtils jwtUtils)
    : IUserService
{
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
            IsActive = registerDto.IsActive
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
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException("Пользователь не найден");
        }
        
        user.Name = updateUserDto.Name;
        user.Bio = updateUserDto.Bio;
        user.BirthDate = updateUserDto.BirthDate;
        user.Location = updateUserDto.Location;

        await userRepository.UpdateUserAsync(user);
    }

    public async Task<IEnumerable<User>> GetSubscribersAsync(int userId)
    {
        return await userRepository.GetSubscribersAsync(userId);
    }

    public async Task<IEnumerable<User>> GetSubscribedUsersAsync(int userId)
    {
        return await userRepository.GetSubscribedUsersAsync(userId);
    }
}
