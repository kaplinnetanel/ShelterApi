using ShelterApi.DTO;
using ShelterApi.Models;
using System.Collections.Generic;
using System.Globalization;
namespace ShelterApi.Repositories;

public interface IMyRepository
{
    Task<IEnumerable<DtoAllSheltersWithArea>>GetAllSheltersWithAreAsync();
    Task<IEnumerable<SearchDto>> GetSearch(string? city = null, int? minCapacity = null, bool? isAccessible = null, bool? isPublic = null);
    Task<IEnumerable<SortedDto>> GetSorted(string? sortBy = null, bool ascending = true);
    Task<IEnumerable<InspectionsDetailedDto>> Getinspections();
    Task<IEnumerable<WithInspectionCountDTO>> GetwithInspectionCount();
    Task<IEnumerable<DtoInspectionsFailed>> GetInspectionsFailed();
    Task<IEnumerable<AreaStatisticsDto>> GetAreasStatistics(); 
}


   
