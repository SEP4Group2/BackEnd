using System.ComponentModel.DataAnnotations;

namespace Domain.Model;

public class Device
{
    [Key] 
    
    public int DeviceId { get; set; }
    
    public bool Status { get; set; }

    public Plant Plant { get; set; }

}