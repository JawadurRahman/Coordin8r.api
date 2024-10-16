public class TimeRange
{
    public int Id { get; set; } // Primary key for TimeRange
    public DateTime StartTime { get; set; } // Start time of the availability
    public DateTime EndTime { get; set; }   // End time of the availability

    public int AvailabilityId { get; set; } // Foreign key to Availability
    public Availability Availability { get; set; } // Navigation property
}
