namespace ShelterApi.DTO
{
    public class AreaStatisticsDto
    {
        public string City { get; set; } = string.Empty;
        public string Neighborhood { get; set; } = string.Empty;
        public int ShelterCount { get; set; }
        public int TotalCapacity { get; set; }
    }
}
