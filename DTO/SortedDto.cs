using System.ComponentModel.DataAnnotations;

namespace ShelterApi.DTO;

public class SortedDto
{
    public string name { get; set; } = string.Empty;
    [Required]
    [StringLength(100)]
    public string Street { get; set; } = string.Empty;
    [Required]
    [StringLength(20)]
    public string BuildingNumber { get; set; } = string.Empty;
    [Range(1, 10000)]
    public int Capacity { get; set; }
    [Required]
    public bool IsAccessible { get; set; } = false;
    [Required]
    public bool IsPublic { get; set; } = false;
  
}
