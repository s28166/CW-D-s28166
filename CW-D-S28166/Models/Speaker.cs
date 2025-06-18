using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CW_D_S28166.Models;

[Table("Speaker")]
public class Speaker
{
    [Key]
    public int IdSpeaker { get; set; }
    
    [MaxLength(30)]
    public string FirstName { get; set; }
    
    [MaxLength(30)]
    public string LastName { get; set; }
    
    [MaxLength(30)]
    public string Email { get; set; }
    
    public virtual ICollection<EventSpeaker> EventSpeakers { get; set; }
}