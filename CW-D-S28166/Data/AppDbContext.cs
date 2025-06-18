using CW_D_S28166.Models;
using Microsoft.EntityFrameworkCore;

namespace CW_D_S28166.Data;

public class AppDbContext : DbContext
{
    public DbSet<Event> Events { get; set; }
    
    public DbSet<Participant> Participants { get; set; }
    
    public DbSet<Speaker> Speakers { get; set; }
    
    public DbSet<EventSpeaker> EventSpeakers { get; set; }
    
    public DbSet<Registration> Registrations { get; set; }
    
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
    }
}