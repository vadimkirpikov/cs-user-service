using CsApi.Models.Domain;
using CsApi.Models.Dto;
using CsApi.Repositories.Interfaces;
using CsApi.Services.Implementations;
using CsApi.Utils.Interfaces;
using Moq;
using Xunit;

namespace CsApi.Tests;


public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IJwtUtils> _mockJwtUtils;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockJwtUtils = new Mock<IJwtUtils>();
        _service = new UserService(_mockRepository.Object, _mockPasswordHasher.Object, _mockJwtUtils.Object);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldThrowException_WhenEmailExists()
    {
        var registerDto = new RegisterDto { Name = "TestName", Email = "test@example.com", Password = "password", Location = "TestLocation", BirthDate = DateTime.Now.AddYears(-20) };
        _mockRepository
            .Setup(repo => repo.GetUserByEmailAsync(registerDto.Email))
            .ReturnsAsync(new User
            {
                Email = registerDto.Email,
                PasswordHash = "TestHash",
                Name = registerDto.Name,
                BirthDate = registerDto.BirthDate,
                Location = registerDto.Location
            });
        
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.RegisterUserAsync(registerDto));
        Assert.Equal("Пользователь с таким email уже существует", exception.Message);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldCallCreateUser_WhenEmailNotExists()
    {
        var registerDto = new RegisterDto
        {
            Email = "test@example.com",
            Password = "password",
            Name = "Test User",
            BirthDate = new DateTime(2000, 1, 1),
            Location = "Earth",
            Bio = "Test Bio",
            IsActive = true
        };
        _mockRepository
            .Setup(repo => repo.GetUserByEmailAsync(registerDto.Email))
            .ReturnsAsync((User?)null);
        _mockPasswordHasher
            .Setup(hasher => hasher.HashPassword(registerDto.Password))
            .Returns("hashed_password");
        
        await _service.RegisterUserAsync(registerDto);
        
        _mockRepository.Verify(repo => repo.CreateUserAsync(It.Is<User>(user =>
            user.Email == registerDto.Email &&
            user.PasswordHash == "hashed_password" &&
            user.Name == registerDto.Name &&
            user.BirthDate == registerDto.BirthDate &&
            user.Location == registerDto.Location &&
            user.Bio == registerDto.Bio &&
            user.IsActive == registerDto.IsActive
        )), Times.Once);
    }

    [Fact]
    public async Task LoginUserAsync_ShouldThrowException_WhenUserNotFound()
    {
        var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };
        _mockRepository
            .Setup(repo => repo.GetUserByEmailAsync(loginDto.Email))
            .ReturnsAsync((User?)null);
        
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.LoginUserAsync(loginDto));
        Assert.Equal("Неверный email или пароль", exception.Message);
    }

    [Fact]
    public async Task LoginUserAsync_ShouldThrowException_WhenPasswordInvalid()
    {
        var loginDto = new LoginDto { Email = "test@example.com", Password = "wrong_password" };
        var user = new User
        {
            Email = loginDto.Email,
            PasswordHash = "hashed_password",
            Name = "TestName",
            BirthDate = DateTime.Now.AddYears(-20),
            Location = "TestLocation",
        };

        _mockRepository
            .Setup(repo => repo.GetUserByEmailAsync(loginDto.Email))
            .ReturnsAsync(user);
        _mockPasswordHasher
            .Setup(hasher => hasher.VerifyPassword(loginDto.Password, user.PasswordHash!))
            .Returns(false);
        
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.LoginUserAsync(loginDto));
        Assert.Equal("Неверный email или пароль", exception.Message);
    }

    [Fact]
    public async Task LoginUserAsync_ShouldReturnJwtToken_WhenCredentialsValid()
    {
        var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };
        var user = new User
        {
            Email = loginDto.Email,
            PasswordHash = "hashed_password",
            Name = "TestName",
            BirthDate = DateTime.Now.AddYears(-20),
            Location = "TestLocation",
        };
        var expectedToken = "jwt_token";

        _mockRepository
            .Setup(repo => repo.GetUserByEmailAsync(loginDto.Email))
            .ReturnsAsync(user);
        _mockPasswordHasher
            .Setup(hasher => hasher.VerifyPassword(loginDto.Password, user.PasswordHash!))
            .Returns(true);
        _mockJwtUtils
            .Setup(jwt => jwt.GenerateJwtToken(user))
            .Returns(expectedToken);
        
        var result = await _service.LoginUserAsync(loginDto);
        
        Assert.Equal(expectedToken, result);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldThrowException_WhenUserNotFound()
    {
        var userId = 1;
        var updateUserDto = new UpdateUserDto
        {
            Name = "Updated Name",
            Bio = "TestBio",
            BirthDate = default,
            Location = "TestLocation",
        };
        _mockRepository
            .Setup(repo => repo.GetUserByIdAsync(userId))
            .ReturnsAsync((User?)null);
        
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateUserAsync(userId, updateUserDto));
        Assert.Equal("Пользователь не найден", exception.Message);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldUpdateUser_WhenUserExists()
    {
        var userId = 1;
        var updateUserDto = new UpdateUserDto
        {
            Name = "Updated Name",
            Bio = "Updated Bio",
            BirthDate = new DateTime(2000, 1, 1),
            Location = "Updated Location"
        };
        var existingUser = new User
        {
            Id = userId,
            Email = "test@example.com",
            PasswordHash = "jlajlas",
            Name = "TestName",
            BirthDate = DateTime.Now.AddYears(-20),
            Location = "TestLocation",
        };

        _mockRepository
            .Setup(repo => repo.GetUserByIdAsync(userId))
            .ReturnsAsync(existingUser);
        
        await _service.UpdateUserAsync(userId, updateUserDto);
        
        _mockRepository.Verify(repo => repo.UpdateUserAsync(It.Is<User>(user =>
            user.Id == userId &&
            user.Name == updateUserDto.Name &&
            user.Bio == updateUserDto.Bio &&
            user.BirthDate == updateUserDto.BirthDate &&
            user.Location == updateUserDto.Location
        )), Times.Once);
    }
}