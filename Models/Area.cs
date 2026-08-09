using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ShelterApi.Models
{
    public class Area
    {
        public int Id { get; set;}
        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string Neighborhood { get; set; } = string.Empty;
        [Required]
        [StringLength(20)]
        public string AreaCode { get; set; } = string.Empty;
        public int RiskLevel { get; set; }

        public ICollection<Shelter> Shelters { get; set; } = new List<Shelter>(); 
    }
}
