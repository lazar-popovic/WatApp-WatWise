using API.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace API.Models.Entity;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base( options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(p => p.Role)
            .WithMany(c => c.Users)
            .HasForeignKey(p => p.RoleId);
        
        modelBuilder.Entity<User>()
            .HasOne(p => p.Location)
            .WithMany(c => c.Users)
            .HasForeignKey(p => p.LocationId);
    }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Role> Roles { get; set; }

    public DbSet<Location> Locations { get; set; }
}