namespace CW_D_S28166.DTOs;

public class EventGetDto
{
    public int IdEvent  { get; set; }
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ICollection<EventSpeakerGetDto> Speakers { get; set; }
    public int RegistrationsCount { get; set; }
    public int FreeSpots { get; set; }
}

public class EventSpeakerGetDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}