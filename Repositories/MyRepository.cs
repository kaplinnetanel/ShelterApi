using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using ShelterApi.date;
using ShelterApi.DTO;
using ShelterApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ShelterApi.Repositories;

public class MyRepository : IMyRepository
{
    private readonly ApplicationDbContext _context;

    public MyRepository(ApplicationDbContext context)
    {
        _context = context;
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
        if(isPublic.HasValue)
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
            city= p.Area.City
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


}
