using CsApi.Utils.Interfaces;

namespace CsApi.Utils.Implementations;

using Microsoft.AspNetCore.Identity;


public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();

    public string HashPassword(string password)
    {
        return _hasher.HashPassword(new object(), password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        var result = _hasher.VerifyHashedPassword(new object(), hashedPassword, password);
        return result == PasswordVerificationResult.Success;
    }
}
