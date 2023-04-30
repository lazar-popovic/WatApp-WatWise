﻿using Microsoft.EntityFrameworkCore;

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
    public DbSet<RefreshToken> RefreshTokens{ get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<DeviceEnergyUsage> DeviceEnergyUsage { get; set; }
    public DbSet<DeviceType> DeviceTypes { get; set; }
    public DbSet<DeviceSubtype> DeviceSubtypes { get; set; }
    public DbSet<DeviceJob> DeviceJobs { get; set; }
}