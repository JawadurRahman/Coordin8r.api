public class Availability
{
    public int Id { get; set; }
    public string Participant { get; set; } // Name of the participant

    public int EventId { get; set; } // Foreign key to Event
    public Event Event { get; set; } // Navigation property
    
    public List<TimeRange> AvailableTimes { get; set; } = new(); // Times they are available
}
