using CW_D_S28166.DTOs;
using CW_D_S28166.Models;
using CW_D_S28166.Services;
using Microsoft.AspNetCore.Mvc;

namespace CW_D_S28166.Controllers;

[ApiController]
// [Route("[controller]")]
public class AppController(IDbService service) : ControllerBase
{
    // 1. Utworzenie nowego wydarzenia
    //     Wprowadzenie danych: tytuł, opis, data, maksymalna liczba uczestników.
    //     Data wydarzenia nie może być przeszła.
    [HttpPost("event")]
    public async Task<IActionResult> AddNewEventAsync([FromBody] EventCreateDto newEvent)
    {
        return Ok(await service.CreateNewEventAsync(newEvent));
    }
    
    // 2. Przypisanie prelegenta do wydarzenia
    //     Możliwość przypisania wielu prelegentów do jednego wydarzenia.
    //     Prelegent nie może być przypisany do dwóch wydarzeń w tym samym czasie.
    [HttpPost]
    [Route("event/{id:int}/addSpeaker")]
    public async Task<IActionResult> AssignSpeakerToEventAsync(
        [FromRoute] int id, [FromBody] SpeakerCreateDto speaker)
    {
        return Ok(await service.AssignSpeakerToEventAsync(id, speaker));
    }
    
    // 3. Rejestracja uczestnika na wydarzenie
    //     Sprawdzenie limitu miejsc – jeśli limit osiągnięty, rejestracja niemożliwa.
    //     Uczestnik może być zarejestrowany tylko raz na dane wydarzenie.
    [HttpPost]
    [Route("event/{id:int}/register")]
    public async Task<IActionResult> RegisterParticipantToEventAsync(
        [FromRoute] int id, [FromBody] ParticipantCreateDto participant)
    {
        return Ok(await service.RegisterParticipantToEventAsync(id, participant));
    }
    
    // 4. Anulowanie rejestracji uczestnika
    //     Uczestnik może anulować swój udział do 24 godzin przed rozpoczęciem wydarzenia.
    [HttpPut]
    [Route("event/{eventId:int}/cancel/{participantId:int}")]
    public async Task<IActionResult> CancelRegistrationAsync(
        [FromRoute] int eventId, [FromRoute] int participantId)
    {
        return Ok(await service.CancelRegistrationAsync(eventId, participantId));
    }
    
    // 5. Pobranie listy wydarzeń z informacją o liczbie wolnych miejsc
    //     Endpoint powinien zwracać wszystkie nadchodzące wydarzenia wraz z:
    //         nazwami prelegentów,
    //         liczbą zarejestrowanych uczestników,
    //         liczbą wolnych miejsc.
    [HttpGet]
    [Route("event/upcoming")]
    public async Task<IActionResult> GetUpcomingEventsAsync()
    {
        return Ok(await service.GetUpcomingEventsAsync());
    }
    
    // 6. Wygenerowanie raportu udziału uczestników
    //     Dla danego uczestnika zwróć wszystkie wydarzenia, w których brał udział, z datami i nazwiskami prelegentów.
    [HttpGet]
    [Route("participant/{participantId:int}/report")]
    public async Task<IActionResult> GetParticipantReportAsync(
        [FromRoute] int participantId)
    {
        return Ok(await service.GetParticipantReportAsync(participantId));
    }
}