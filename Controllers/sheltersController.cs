using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterApi.DTO;
using ShelterApi.Models;
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
    [HttpGet("{id}")]
    public async Task<ActionResult<Shelter?>> GetById(int id)
    {
        var respons = await _conect.GetById(id);
        if (respons == null)
        {
            return NotFound();
        }
        return Ok(respons);

    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletShelter(int id)
    {
        var respons = await _conect.DeletShelter(id);
        if(respons == false)
        {
            return  NotFound();

        }
        return NoContent();

    }
    [HttpPost]
    public async Task<IActionResult> CreateShelterAsync(Shelter shelter)
    {
        var r = _conect.CreateShelterAsync(shelter);
        if(r == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpPut]
    public async Task<IActionResult>UpdateShelter(int id,ShelterDto shelter)
    {
        var r = await _conect.UpdateShelterAsync(id,shelter);
        if (r == false)
        {
            return NotFound();
        }
        return CreatedAtAction(nameof(GetById), new { id = shelter.Name }, shelter);
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
    [HttpGet("average-score-by-type")]
    public async Task<ActionResult<IEnumerable<AverageScoreByTypeDto>>> GetaverageScoreByType()
    {
        var respons = await _conect.GetaverageScoreByType();
        return Ok(respons);
    }

}