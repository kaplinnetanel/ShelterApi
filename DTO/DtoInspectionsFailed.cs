namespace ShelterApi.DTO
{
    public class DtoInspectionsFailed
    {
        public int inspectionId { get; set;  }
        public DateTime inspectionDate { get; set; }
        public int readinessScore { get; set; }
        public int defectsCount { get; set; }
        public string shelterName { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        
    }
}
