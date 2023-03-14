using Microsoft.EntityFrameworkCore;

namespace API.Models.Entity;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base( options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Role> Roles { get; set; }

    public DbSet<Location> Locations { get; set; }
    public DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }
}