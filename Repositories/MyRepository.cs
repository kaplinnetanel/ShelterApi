using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using ShelterApi.date;
using ShelterApi.DTO;
using ShelterApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace ShelterApi.Repositories;

public class MyRepository : IMyRepository
{
    private readonly ApplicationDbContext _context;

    public MyRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Shelter?> GetById(int id)
    {
        return await _context.Shelters.FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task<bool> DeletShelter(int id)

    {
        var d = await _context.Shelters.FindAsync(id);
        if (d == null )
        {
            return false;
        }
        _context.Shelters.Remove(d);
        return true;
    }
    public async Task<int?> CreateShelterAsync(Shelter shelter)
    {
        if (shelter.IsPublic == true)
        {
            return null;
        }
        await _context.Shelters.AddAsync(shelter);
        await _context.SaveChangesAsync();
        return shelter.Id;
    }
    public async Task<bool> UpdateShelterAsync(int id, ShelterDto updatedShelter)
    {
        var shelter = await _context.Shelters.FirstOrDefaultAsync(p=> p.Id==id);
        if (shelter == null)
        {
            return false;
        }
        shelter.Name = updatedShelter.Name;
        shelter.Street = updatedShelter.Street;
        shelter.BuildingNumber = updatedShelter.BuildingNumber;
        shelter.Capacity = updatedShelter.Capacity;
        shelter.IsAccessible = updatedShelter.IsAccessible;
        shelter.IsPublic = updatedShelter.IsPublic;
        shelter.ShelterType = updatedShelter.ShelterType;
        shelter.AreaId = updatedShelter.AreaId;
        await _context.SaveChangesAsync();

        return true;
    }



    public async Task<IEnumerable<DtoAllSheltersWithArea>> GetAllSheltersWithAreAsync()
    {
        return await _context.Shelters.Select(
            p => new DtoAllSheltersWithArea
            {
                shelterId = p.Id,
                shelterName = p.Name,
                capacity = p.Capacity,
                city = p.Area.City,
                neighborhood = p.Area.Neighborhood

            }).ToListAsync();
    }
    public async Task<IEnumerable<SearchDto>> GetSearch(string? city = null, int? minCapacity = null, bool? isAccessible = null, bool? isPublic = null)
    {
        IQueryable<Shelter> query = _context.Shelters;
        if (!string.IsNullOrWhiteSpace(city))
        {
            query = query.Where(p => p.Area.City == city);
        }
        if (minCapacity.HasValue)
        {
            query = query.Where(p => p.Capacity > minCapacity.Value);
        }
        if (isAccessible.HasValue)
        {
            query = query.Where(p => p.IsAccessible == isAccessible.Value);

        }
        if (isPublic.HasValue)
        {
            query = query.Where(p => p.IsPublic == isPublic.Value);

        }
        return await query.Select(p => new SearchDto
        {
            Id = p.Id,
            name = p.Name,
            street = p.Street,
            capacity = p.Capacity,
            isAccessible = p.IsAccessible,
            city = p.Area.City
        }
        ).ToListAsync();
    }
    public async Task<IEnumerable<SortedDto>> GetSorted(string? sortBy = "name", bool ascending = true)
    {

        var query = _context.Shelters.Select(p => new
        {
            Shelter = p,
            CityName = p.Area.City
        });
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            sortBy = "name";
        }

        if (sortBy.ToLower() == "capacity")
        {
            if (ascending == true)
            {
                query = query.OrderBy(x => x.Shelter.Capacity);
            }
            else
            {
                query = query.OrderByDescending(x => x.Shelter.Capacity);
            }
        }
        else if (sortBy.ToLower() == "city")
        {
            if (ascending == true)
            {
                query = query.OrderBy(x => x.CityName);
            }
            else
            {
                query = query.OrderByDescending(x => x.CityName);
            }
        }
        else
        {
            if (ascending == true)
            {
                query = query.OrderBy(x => x.Shelter.Name);
            }
            else
            {
                query = query.OrderByDescending(x => x.Shelter.Name);
            }
        }

        return await query.Select(x => new SortedDto
        {
            name = x.Shelter.Name,
            Street = x.Shelter.Street,
            BuildingNumber = x.Shelter.BuildingNumber,
            Capacity = x.Shelter.Capacity,
            IsAccessible = x.Shelter.IsAccessible,
            IsPublic = x.Shelter.IsPublic
        }).ToListAsync();
    }
    public async Task<IEnumerable<InspectionsDetailedDto>> Getinspections()
    {
        return await _context.Inspections.Select
             (p => new InspectionsDetailedDto
             {
                 inspectionId = p.Id,
                 inspectionDate = p.InspectionDate,
                 readinessScore = p.ReadinessScore,
                 passed = p.Passed,
                 shelterScore = p.Shelter.Name,
                 city = p.Shelter.Area.City,
                 neighborhood = p.Shelter.Area.Neighborhood
             }
             ).ToListAsync();
    }
    public async Task<IEnumerable<WithInspectionCountDTO>> GetwithInspectionCount()
    {
        return await _context.Shelters.Select(p => new WithInspectionCountDTO
        {
            shelterId = p.Id,
            shelterName = p.Name,
            inspectionCount = p.Inspections.Count(p => p.Passed)
        }
        ).ToListAsync();
    }
    public async Task<IEnumerable<DtoInspectionsFailed>> GetInspectionsFailed()
    {
        return await _context.Inspections.Where(p => p.Passed).Select(p => new DtoInspectionsFailed
        {
            inspectionId = p.Id,
            inspectionDate = p.InspectionDate,
            readinessScore = p.ReadinessScore,
            defectsCount = p.DefectsCount,
            shelterName = p.Shelter.Name,
            city = p.Shelter.Area.City
        }

            ).ToArrayAsync();
    }
    public async Task<IEnumerable<AreaStatisticsDto>> GetAreasStatistics()
    {
        return await _context.Areas.Select(a => new AreaStatisticsDto
        {
            City = a.City,
            Neighborhood = a.Neighborhood,
            ShelterCount = a.Shelters.Count(),
            TotalCapacity = a.Shelters.Sum(s => (int?)s.Capacity) ?? 0 // מגן מפני ערכי null אם אין מקלטים
        })
        .ToListAsync();
    }
    public async Task<IEnumerable<AverageScoreByTypeDto>> GetaverageScoreByType()
    {
        return await _context.Shelters.Select(p => new AverageScoreByTypeDto
        {
            shelterType = p.ShelterType,
            averageReadinessScore =_context.Shelters.Where(s=> s.ShelterType == p.ShelterType).Average(s=> (double)s.Area.RiskLevel),
            totalInspections = _context.Shelters.Where(s => s.ShelterType == p.ShelterType).Count()

        }).Distinct().ToListAsync();

    }
    public async Task<ShelterPagedResponseDto> GetPagedShelters(int page = 1, int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 1;
        if (pageSize > 50) pageSize = 50;

        int totalCount = await _context.Shelters.CountAsync();
        var items = await _context.Shelters
        .OrderBy(s => s.Name)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(s => new Shelter
        {
            Id = s.Id,
            Name = s.Name,
            Capacity = s.Capacity
        })
        .ToListAsync();

    return new ShelterPagedResponseDto
    {
        Items = items,
        TotalCount = totalCount,
        Page = page,
        PageSize = pageSize,
        TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
    };
}

 






}
