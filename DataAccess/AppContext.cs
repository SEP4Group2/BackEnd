﻿using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess;

public class AppContext : DbContext
{
    public DbSet<Plant> Plants { get; set; }
    public DbSet<PlantPreset> Presets { get; set; }
    public DbSet<PlantData> PlantData { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<User> Users { get; set; }

    
    public AppContext(DbContextOptions<AppContext> options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseInMemoryDatabase("TestDatabase");
        }
    }
}