namespace CW_D_S28166.DTOs;

public class RegistrationGetDto
{
    public int IdParticipant { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime? CancellationDate { get; set; }
}