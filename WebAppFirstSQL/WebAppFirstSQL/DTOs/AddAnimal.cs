using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace WebAppFirstSQL.DTOs;

public class AddAnimal
{
    [Required]
    [MinLength(1)]
    [MaxLength(200)]
    public string Name { get; set; }
    
    [MaxLength(200)]
    public string? Description { get; set; }
    
    [Required]
    [MinLength(1)]
    [MaxLength(200)]
    public string Category { get; set; }
    
    [Required]
    [MinLength(1)]
    [MaxLength(200)]
    public string Area { get; set; }
}