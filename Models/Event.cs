public class Event
{
    public int Id { get; set; } // Event ID
    public string Title { get; set; } // Title of the event
    public string Organizer { get; set; } // Name of the event creator
    public List<Availability> Availabilities { get; set; } = new(); // List of participant availabilities
    public string? FinalSelectedTime { get; set; } // Time selected by the organizer
}
