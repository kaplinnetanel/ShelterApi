namespace ShelterApi.DTO;

public class AverageScoreByTypeDto
{
    public string shelterType { get; set; } = string.Empty;
    public double averageReadinessScore { get; set; }
    public int totalInspections { get; set; }
}
