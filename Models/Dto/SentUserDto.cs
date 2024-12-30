namespace CsApi.Models.Dto;

public class SentUserDto
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public string? Bio { get; set; }
    public required DateTime? BirthDate { get; set; }
    public required string? Location { get; set; }
    public bool IsActive { get; set; }
}