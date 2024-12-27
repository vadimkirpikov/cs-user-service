using CsApi.Models.Domain;

namespace CsApi.Utils.Interfaces;

public interface IJwtUtils
{
    string GenerateJwtToken(User user);
}