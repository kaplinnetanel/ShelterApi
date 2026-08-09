namespace ShelterApi.DTO;

public class SearchDto
{
    public int Id { get; set; }
    public string name { get; set; } = string.Empty;
    public string street { get; set; } = string.Empty;
    public int? capacity { get; set; }
    public bool? isAccessible { get; set; }
    public string? city { get; set; }
}
