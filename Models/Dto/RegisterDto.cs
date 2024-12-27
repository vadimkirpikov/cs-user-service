using System.ComponentModel.DataAnnotations;

namespace CsApi.Models.Dto;

public class RegisterDto
{
    [EmailAddress]
    [MaxLength(50)]
    public required string Email { get; set; }

    [MaxLength(50)]
    public required string Password { get; set; }

    [MaxLength(50)]
    public required string Name { get; set; }

    public required DateTime BirthDate { get; set; }

    [MaxLength(30)]
    public required string Location { get; set; }
    
    [MaxLength(1000)]
    public string? Bio { get; set; }

    public bool IsActive { get; set; } = true;
}
