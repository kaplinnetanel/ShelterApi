using Microsoft.AspNetCore.Mvc;
using ShelterApi.DTO;
using ShelterApi.Repositories;
using System.Diagnostics.Contracts;

namespace ShelterApi.Controllers;

[ApiController]
[Route("[controller]")]
public class sheltersController : ControllerBase
{
    private readonly IMyRepository _conect;
    public sheltersController(IMyRepository MyRepository)
    {
        _conect = MyRepository;
    }
    [HttpGet("/with-area")]

    public async Task<ActionResult<IEnumerable<DtoAllSheltersWithArea>>> GetAllSheltersWithAreAsync()
    {
        var respons =await _conect.GetAllSheltersWithAreAsync();
        return Ok(respons);
    }
    [HttpGet("/search")]
    public async Task<ActionResult<IEnumerable<SearchDto>>> GetSearchas(string? city = null, int? minCapacity = null, bool? isAccessible = null, bool? isPublic = null)
    {
        var respons = await _conect.GetSearch(city, minCapacity , isAccessible , isPublic);
        return Ok(respons);
    }
    [HttpGet("/sorted")]
    public async Task<ActionResult<IEnumerable<SortedDto>>> GetSortedv(string? sortBy = null, bool ascending = true)
    {
        var respons = await _conect.GetSorted(sortBy, ascending);
        return Ok(respons);
    }



}