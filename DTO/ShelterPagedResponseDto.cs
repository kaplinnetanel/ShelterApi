
using ShelterApi.Models;
namespace ShelterApi.DTO;

public class ShelterPagedResponseDto
{

    public List<Shelter> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}
