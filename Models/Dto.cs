public class EventDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Organizer { get; set; }
    public List<AvailabilityDto> Availabilities { get; set; } // Include only what's needed
}

public class AvailabilityDto
{
    public string Participant { get; set; } // Name of the participant
    public List<TimeRangeDto> AvailableTimes { get; set; } // List of available times
}


public class TimeRangeDto
{
    public DateTime StartTime { get; set; } // Start time of availability
    public DateTime EndTime { get; set; } // End time of availability
}
