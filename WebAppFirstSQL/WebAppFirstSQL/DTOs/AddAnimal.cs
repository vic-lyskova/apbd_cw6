using System.ComponentModel.DataAnnotations;

namespace WebAppFirstSQL.DTOs;

public class AddAnimal
{
    [Required]
    [MinLength(1)]
    [MaxLength(200)]
    public string Name { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }
}