using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [HttpGet("inspections/detailed")]
    public async Task<ActionResult<IEnumerable<InspectionsDetailedDto>>> Getinspections()
    {
        var respons = await _conect.Getinspections();
        return Ok(respons);
    }
    [HttpGet("with-inspection-count")]
    public async Task<ActionResult<IEnumerable<WithInspectionCountDTO>>>GetwithInspectionCount()
    {
        var respons = await _conect.GetwithInspectionCount();
        return Ok(respons);

    }
    [HttpGet("inspections/failed")]
    public async Task<ActionResult<IEnumerable<DtoInspectionsFailed>>> GetInspectionsFailed()
    {
        var respons = await _conect.GetInspectionsFailed();
        return Ok(respons);
    }
    [HttpGet("statistics")]
    public async Task<ActionResult<IEnumerable<AreaStatisticsDto>>> GetAreasStatistics()
    {
        var statistics = await _conect.GetAreasStatistics();
        return Ok(statistics);
    }

}