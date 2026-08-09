using System.Runtime.InteropServices;

namespace ShelterApi.DTO;

public class DtoAllSheltersWithArea
{
    public int shelterId { get; set; }
    public string shelterName { get; set; } = string.Empty;
    public int capacity { get; set; }
    public string city { get; set; } = string.Empty;
    public string neighborhood { get; set; } = string.Empty;


}
