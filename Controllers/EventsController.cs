using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly Coordin8rDbContext _context;

    public EventsController(Coordin8rDbContext context)
    {
        _context = context;
    }

    // Create an event
    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] Event newEvent)
    {
        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();
        return Ok(newEvent);
    }

    // Delete an event
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem == null) return NotFound("Event not found");
        _context.Events.Remove(eventItem);
        await _context.SaveChangesAsync();
        return Ok(eventItem);
    }

    // GET: api/events
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
    {
        var events = await _context.Events
            .Include(e => e.Availabilities) // Include availabilities
                .ThenInclude(a => a.AvailableTimes) // Include available times
            .ToListAsync();
        
        // Map to DTOs
        var eventDtos = events.Select(e => new EventDto
        {
            Id = e.Id,
            Title = e.Title,
            Organizer = e.Organizer,
            Availabilities = e.Availabilities.Select(a => new AvailabilityDto
            {
                Participant = a.Participant,
                AvailableTimes = a.AvailableTimes.Select(t => new TimeRangeDto
                {
                    StartTime = t.StartTime,
                    EndTime = t.EndTime
                }).ToList()
            }).ToList()
        }).ToList();

        return Ok(eventDtos);
    }

    // Test api
    [HttpGet("hello")]
    public async Task<IActionResult> GetHello()
    {
        return Ok("Hello");
    }

    // Submit availability for an event
    [HttpPost("{id}/availability")]
    public async Task<IActionResult> SubmitAvailability(int id, [FromBody] AvailabilityDto availabilityDto)
    {
        // Find the event by the provided ID
        var eventItem = await _context.Events
            .Include(e => e.Availabilities) // Include existing availabilities
            .ThenInclude(a => a.AvailableTimes)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (eventItem == null) return NotFound("Event not found");

        // Create a new Availability instance from the DTO
        var availability = new Availability
        {
            Participant = availabilityDto.Participant,
            EventId = eventItem.Id, // Set the EventId based on the found event
            AvailableTimes = availabilityDto.AvailableTimes.Select(t => new TimeRange
            {
                StartTime = t.StartTime,
                EndTime = t.EndTime
            }).ToList() // Convert DTO times to entity times
        };

        // Add the new availability to the event's availabilities
        eventItem.Availabilities.Add(availability);

        await _context.SaveChangesAsync(); // Save changes

        var eventDto = new EventDto
        {
            Id = eventItem.Id,
            Title = eventItem.Title,
            Organizer = eventItem.Organizer,
            Availabilities = eventItem.Availabilities.Select(a => new AvailabilityDto
            {
                Participant = a.Participant,
                AvailableTimes = a.AvailableTimes.Select(t => new TimeRangeDto
                {
                    StartTime = t.StartTime,
                    EndTime = t.EndTime
                }).ToList()
            }).ToList()
        };

        return Ok(eventDto); // Return the updated event
    }



    // Select the final time for the event
    [HttpPost("{id}/finalize")]
    public async Task<IActionResult> FinalizeEventTime(int id, [FromBody] string finalTime)
    {
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem == null) return NotFound("Event not found");

        eventItem.FinalSelectedTime = finalTime;
        await _context.SaveChangesAsync();

        // (Optional) Send email notifications here

        return Ok(eventItem);
    }
}
