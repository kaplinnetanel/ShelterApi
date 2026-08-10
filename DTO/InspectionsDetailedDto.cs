namespace ShelterApi.DTO;

public class InspectionsDetailedDto
{
    public int inspectionId { get; set; }
    public DateTime inspectionDate { get; set; }
    public int readinessScore { get; set; }
    public bool passed { get; set; }
    public string shelterScore { get; set; } = string.Empty;
    public string city { get; set; } = string.Empty;
    public string neighborhood { get; set; } = string.Empty;
    

}
