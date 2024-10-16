using Microsoft.EntityFrameworkCore;

public class Coordin8rDbContext : DbContext
{
    public Coordin8rDbContext(DbContextOptions<Coordin8rDbContext> options) : base(options)
    {
    }
    public DbSet<Event> Events { get; set; }
    public DbSet<Availability> Availabilities { get; set; }
    public DbSet<TimeRange> TimeRanges { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // One-to-Many relationship between Availability and TimeRange
        modelBuilder.Entity<TimeRange>()
            .HasOne(tr => tr.Availability)
            .WithMany(a => a.AvailableTimes)
            .HasForeignKey(tr => tr.AvailabilityId)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-Many relationship between Event and Availability
        modelBuilder.Entity<Availability>()
            .HasOne(a => a.Event)
            .WithMany(e => e.Availabilities)
            .HasForeignKey(a => a.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
