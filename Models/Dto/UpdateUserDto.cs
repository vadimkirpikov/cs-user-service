using System.ComponentModel.DataAnnotations;

namespace CsApi.Models.Dto;

public class UpdateUserDto
{
    [MaxLength(50)]
    public required string Name { get; set; }

    [MaxLength(1000)]
    public required string Bio { get; set; }

    public required DateTime BirthDate { get; set; }

    [MaxLength(30)]
    public required string Location { get; set; }
}