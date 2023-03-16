﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entity;

public class User
{
    [Key]
    public int Id { get; set; }
    public string? Email { get; set; } = string.Empty;
    public string? PasswordHash { get; set; } = string.Empty;
    public string? Firstname { get; set; } = string.Empty;
    public string? Lastname { get; set; } = string.Empty;
    public bool? Verified { get; set; }
    public int? RoleId { get; set; } = 3;
    [ForeignKey("RoleId")]
    public Role? Role { get; set; }
    public int? LocationId { get; set; }
    [ForeignKey("LocationId")]
    public Location? Location { get; set; }
}

added migration,created table via DbSet,