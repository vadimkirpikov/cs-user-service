using System.ComponentModel.DataAnnotations;

namespace CsApi.Models.Dto;

public class LoginDto
{
    [EmailAddress]
    [MaxLength(50)]
    public required string Email { get; set; }

    [MaxLength(50)]
    public required string Password { get; set; }
}
