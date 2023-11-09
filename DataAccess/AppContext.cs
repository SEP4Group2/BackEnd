using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess;

public class AppContext : DbContext
{
    // private readonly IConfiguration configuration;
    // public AppContext(IConfiguration configuration)
    // {
    //     this.configuration = configuration;
    // }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=tina;");
    }

    public DbSet<Plant> Plants { get; set; }
    public DbSet<PlantPreset> Presets { get; set; }

    // I am not sure if we need this 
    
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<PlantPreset>().HasKey(preset => preset.PresetId);
    //     modelBuilder.Entity<Plant>().HasKey(plant => plant.PlantId);
    // }
}