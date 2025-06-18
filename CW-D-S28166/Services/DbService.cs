using CW_D_S28166.Data;
using CW_D_S28166.DTOs;
using CW_D_S28166.Exceptions;
using CW_D_S28166.Models;
using Microsoft.EntityFrameworkCore;
using InvalidDataException = CW_D_S28166.Exceptions.InvalidDataException;

namespace CW_D_S28166.Services;

public interface IDbService
{
    public Task<Event> CreateNewEventAsync(EventCreateDto newEvent);

    public Task<EventSpeaker> AssignSpeakerToEventAsync(int id, SpeakerCreateDto speakerCreateDto);
    
    public Task<RegistrationGetDto> RegisterParticipantToEventAsync(int id, ParticipantCreateDto participantCreateDto);
    
    public Task<RegistrationGetDto> CancelRegistrationAsync(int eventId, int participantId);
    
    public Task<ICollection<EventGetDto>> GetUpcomingEventsAsync();
    
    public Task<ICollection<RegistrationFullGetDto>> GetParticipantReportAsync(int participantId);
}

public class DbService(AppDbContext data) : IDbService
{
    public async Task<Event> CreateNewEventAsync(EventCreateDto newEvent)
    {
        if (newEvent.StartDate < DateTime.Now)
            throw new InvalidDataException("Start date cannot be earlier than now");
        
        if (newEvent.EndDate < newEvent.StartDate)
            throw new InvalidDataException("End date cannot be earlier than start date");
        
        // Dodatek: Jednak ktoś musi tam być
        if (newEvent.MaxParticipants <= 10)
            throw new InvalidDataException("Max participants cannot be less or equal to 10");


        var eventToCreate = new Event
        {
            // IdEvent = data.Events.Max(e => e.IdEvent) + 1,
            Title = newEvent.Title,
            Description = newEvent.Description,
            StartDate = newEvent.StartDate,
            EndDate = newEvent.EndDate,
            MaxParticipants = newEvent.MaxParticipants,
        };
        
        await data.Events.AddAsync(eventToCreate);
        await data.SaveChangesAsync();
        
        return eventToCreate;
    }

    public async Task<EventSpeaker> AssignSpeakerToEventAsync(int id, SpeakerCreateDto speakerCreateDto)
    {
        var eventToUpdate = await data.Events.FindAsync(id);
        if (eventToUpdate == null)
            throw new NotFoundException("Event not found");
        
        var speakerToAdd = await data.Speakers.FirstOrDefaultAsync(s => s.FirstName == speakerCreateDto.FirstName && s.LastName == speakerCreateDto.LastName);

        if (speakerToAdd == null)
        {
            speakerToAdd = new Speaker
            {
                // IdSpeaker = data.Speakers.Max(s => s.IdSpeaker) + 1,
                FirstName = speakerCreateDto.FirstName,
                LastName = speakerCreateDto.LastName,
                Email = speakerCreateDto.Email,
            };
            await data.Speakers.AddAsync(speakerToAdd);
            await data.SaveChangesAsync();
        }

        if (await data.EventSpeakers
                .Include(es => es.Event)
                .AnyAsync(es => es.IdSpeaker == speakerToAdd.IdSpeaker &&
                                es.Event.StartDate < eventToUpdate.EndDate
                                && es.Event.EndDate > eventToUpdate.StartDate))
            throw new InvalidDataException("Speaker is already assigned to event in this time");
        
        if (!await data.EventSpeakers.AnyAsync(es => es.IdEvent == id && es.IdSpeaker == speakerToAdd.IdSpeaker))
        {
            data.EventSpeakers.Add(new EventSpeaker
            {
                IdEvent = id,
                IdSpeaker = speakerToAdd.IdSpeaker,
            });
            
            await data.SaveChangesAsync();
        }
        
        return new EventSpeaker{
            IdEvent = id,
            IdSpeaker = speakerToAdd.IdSpeaker,
        };
    }

    public async Task<RegistrationGetDto> RegisterParticipantToEventAsync(int id, ParticipantCreateDto participantCreateDto)
    {
        var eventToUpdate = await data.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.IdEvent == id);

        if (eventToUpdate == null)
            throw new NotFoundException("Event not found");

        var participantToAdd = await data.Participants.FirstOrDefaultAsync(p =>
            p.FirstName == participantCreateDto.FirstName &&
            p.LastName == participantCreateDto.LastName &&
            p.Email == participantCreateDto.Email);

        if (participantToAdd == null)
        {
            participantToAdd = new Participant
            {
                FirstName = participantCreateDto.FirstName,
                LastName = participantCreateDto.LastName,
                Email = participantCreateDto.Email,
            };

            await data.Participants.AddAsync(participantToAdd);
            await data.SaveChangesAsync();
        }

        if (eventToUpdate.Registrations.Count(r => !r.IsCanceled) >= eventToUpdate.MaxParticipants)
            throw new InvalidDataException("Maximum participants reached");

        if (await data.Registrations.AnyAsync(r =>
                r.IdEvent == id &&
                r.IdParticipant == participantToAdd.IdParticipant &&
                !r.IsCanceled))
            throw new InvalidDataException("Participant already registered");

        var reg = new Registration
        {
            IdEvent = id,
            IdParticipant = participantToAdd.IdParticipant,
            RegistrationDate = DateTime.Now,
            IsCanceled = false
        };

        await data.Registrations.AddAsync(reg);
        await data.SaveChangesAsync();

        return new RegistrationGetDto
        {
            IdParticipant = reg.IdParticipant,
            FirstName = participantToAdd.FirstName,
            LastName = participantToAdd.LastName,
            RegistrationDate = reg.RegistrationDate,
            CancellationDate = reg.CancellationDate,
        };
    }

    public async Task<RegistrationGetDto> CancelRegistrationAsync(int eventId, int participantId)
    {
        var reg = await data.Registrations
            .Include(e => e.Event)
            .Include(e => e.Participant)
            .FirstOrDefaultAsync(r => r.IdEvent == eventId && r.IdParticipant == participantId);
        if (reg == null)
            throw new NotFoundException("Registration not found");
        
        if ((reg.Event.StartDate - DateTime.Now).TotalHours < 24)
            throw new InvalidDataException("Cannot cancel registration 24 hours before its beginning");
        
        if (reg.IsCanceled)
            throw new InvalidDataException("Registration is already canceled");

        reg.IsCanceled = true;
        reg.CancellationDate = DateTime.Now;
        await data.SaveChangesAsync();
        
        return new RegistrationGetDto
        {
            IdParticipant = reg.IdParticipant,
            FirstName = reg.Participant.FirstName,
            LastName = reg.Participant.LastName,
            RegistrationDate = reg.RegistrationDate,
            CancellationDate = reg.CancellationDate,
        };
    }

    public async Task<ICollection<EventGetDto>> GetUpcomingEventsAsync()
    {
        var events = await data.Events
            .Where(e => e.StartDate > DateTime.Now)
            .Include(e => e.EventSpeakers)
            .ThenInclude(es => es.Speaker)
            .Include(e => e.Registrations)
            .Select(e => new EventGetDto
            {
                IdEvent = e.IdEvent,
                Title = e.Title,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Speakers = e.EventSpeakers.Select(es => new EventSpeakerGetDto
                {
                    FirstName = es.Speaker.FirstName,
                    LastName = es.Speaker.LastName,
                }).ToList(),
                RegistrationsCount = e.Registrations.Count(r => !r.IsCanceled),
                FreeSpots = e.MaxParticipants - e.Registrations.Count(r => !r.IsCanceled),
            }).ToListAsync();
        
        return events;
    }

    public async Task<ICollection<RegistrationFullGetDto>> GetParticipantReportAsync(int participantId)
    {
        var report = await data.Registrations
            .Where(r => r.IdParticipant == participantId && !r.IsCanceled)
            .Include(r => r.Participant)
            .Include(r => r.Event)
            .ThenInclude(es => es.EventSpeakers)
            .ThenInclude(es => es.Speaker)
            .Select(r => new RegistrationFullGetDto
            {
                FirstName = r.Participant.FirstName,
                LastName = r.Participant.LastName,
                Title = r.Event.Title,
                RegistrationDate = r.Event.StartDate,
                StartDate = r.Event.StartDate,
                EndDate = r.Event.EndDate,
                Speakers = r.Event.EventSpeakers.Select(es => new EventSpeakerGetDto
                {
                    FirstName = es.Speaker.FirstName,
                    LastName = es.Speaker.LastName,
                }).ToList(),
            }).ToListAsync();
        
        return report;
        // throw new Exception("123");
    }
}