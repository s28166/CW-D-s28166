using System.ComponentModel.DataAnnotations;

namespace CW_D_S28166.DTOs;

public class EventCreateDto
{
    [Required]
    [MaxLength(30)]
    public string Title { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string Description { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    [Required]
    public int MaxParticipants { get; set; }
}