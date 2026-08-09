using System.ComponentModel.DataAnnotations;

namespace ShelterApi.Models
{
    public class Inspection
    {
        public int Id { get; set; }
        [Required]
        public DateTime InspectionDate { get; set; } = DateTime.UtcNow;
        [Range(0,100)]
        public int ReadinessScore { get; set; }
        [Required]
        public bool Passed { get; set; } = false;
        [Range(0,100)]
        public int DefectsCount { get; set; }
        [Required]
        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
        public Shelter Shelter { get; set; } = null!;

        public int ShelterId { get; set; }
    }
}
