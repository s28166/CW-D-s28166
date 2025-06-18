using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CW_D_S28166.Models;

[Table("Event")]
public class Event
{
    [Key]
    public int IdEvent { get; set; }
    
    [MaxLength(30)]
    public string Title { get; set; }
    
    [MaxLength(30)]
    public string Description { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public int MaxParticipants { get; set; }
    
    public virtual ICollection<EventSpeaker> EventSpeakers { get; set; }
    
    public virtual ICollection<Registration> Registrations { get; set; }
}