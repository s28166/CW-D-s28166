using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CW_D_S28166.Models;

[Table("Registration")]
[PrimaryKey(nameof(IdEvent), nameof(IdParticipant))]
public class Registration
{
    public int IdEvent { get; set; }
    
    public int IdParticipant { get; set; }
    
    public DateTime RegistrationDate { get; set; }
    
    public bool IsCanceled { get; set; }
    
    public DateTime? CancellationDate { get; set; }
    
    [ForeignKey("IdEvent")]
    public virtual Event Event { get; set; }
    
    [ForeignKey("IdParticipant")]
    public virtual Participant Participant { get; set; }
}