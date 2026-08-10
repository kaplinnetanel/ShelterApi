namespace ShelterApi.DTO
{
    public class WithInspectionCountDTO
    {
        public int shelterId { get; set; }
        public string shelterName { get; set; } = string.Empty;
        public int inspectionCount { get; set; }

    }
}
