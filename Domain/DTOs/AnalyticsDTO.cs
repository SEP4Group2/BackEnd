namespace Domain.DTOs;

public class AnalyticsDTO
{
   public float avgTemperature { get; set; }
   public float avgHumidity { get; set; }
   public float avgUVLight { get; set; }
   public float avgMoisture { get; set; }
   public DateOnly date { get; set; }
}