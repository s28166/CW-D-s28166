using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CW_D_S28166.Models;

[Table("EventSpeaker")]
[PrimaryKey( nameof(IdEvent), nameof(IdSpeaker))]
public class EventSpeaker
{
    public int IdEvent { get; set; }
    public int IdSpeaker { get; set; }
    
    [ForeignKey("IdEvent")]
    public virtual Event Event { get; set; }
    
    [ForeignKey("IdSpeaker")]
    public virtual Speaker Speaker { get; set; }
}