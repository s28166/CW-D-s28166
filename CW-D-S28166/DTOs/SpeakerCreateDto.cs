using System.ComponentModel.DataAnnotations;

namespace CW_D_S28166.DTOs;

public class SpeakerCreateDto
{
    [Required]
    [MaxLength(30)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string LastName { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string Email { get; set; }
}