using System.ComponentModel.DataAnnotations;

namespace ShelterApi.Models
{
    public class Shelter
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string Streer { get; set; } = string.Empty;
        [Required]
        [StringLength(20)]
        public string BuildingNumber { get; set; } = string.Empty;
        [Range(1,10000)]
       public int Capacity { get; set; }
       [Required]
       public bool IsAccessible { get; set; } = false;
       [Required]
       public bool IsPublic { get; set; } = false;
       [Required]
       [RegularExpression("(PublicBuilding|School|Parking|Residential|Commercial)$")]
       public string ShelterType { get; set; } = string.Empty;
       public int AreaCode { get; set; }
       public Area Area { get; set; } = null!;
        public ICollection<Inspection> Inspections { get; } = new List<Inspection>();






    }
}
