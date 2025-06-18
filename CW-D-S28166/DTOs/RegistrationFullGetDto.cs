namespace CW_D_S28166.DTOs;

public class RegistrationFullGetDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Title { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ICollection<EventSpeakerGetDto> Speakers { get; set; }
    
}