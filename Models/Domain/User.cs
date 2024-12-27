using System.ComponentModel.DataAnnotations;

namespace CsApi.Models.Domain;

public class User
{
    public int Id { get; set; }
    [EmailAddress]
    [MaxLength(50)]
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    [MaxLength(50)]
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [MaxLength(1000)]
    public string? Bio { get; set; }
    public required DateTime? BirthDate { get; set; }
    [MaxLength(30)]
    public required string? Location { get; set; }
    public bool IsActive { get; set; }
    
    public ICollection<Subscription> Subscriptions { get; set; }
    public ICollection<Subscription> Subscribers { get; set; }
}
