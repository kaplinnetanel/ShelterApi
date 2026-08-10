using ShelterApi.DTO;
using ShelterApi.Models;
using System.Collections.Generic;
using System.Globalization;
namespace ShelterApi.Repositories;

public interface IMyRepository
{
    Task<Shelter?> GetById(int id );
    Task<bool> DeletShelter(int id);
    Task<bool> UpdateShelterAsync(int id, ShelterDto updatedShelter);
    Task<int?> CreateShelterAsync(Shelter shelter);
    Task<IEnumerable<DtoAllSheltersWithArea>>GetAllSheltersWithAreAsync();
    Task<IEnumerable<SearchDto>> GetSearch(string? city = null, int? minCapacity = null, bool? isAccessible = null, bool? isPublic = null);
    Task<IEnumerable<SortedDto>> GetSorted(string? sortBy = null, bool ascending = true);
    Task<IEnumerable<InspectionsDetailedDto>> Getinspections();
    Task<IEnumerable<WithInspectionCountDTO>> GetwithInspectionCount();
    Task<IEnumerable<DtoInspectionsFailed>> GetInspectionsFailed();
    Task<IEnumerable<AreaStatisticsDto>> GetAreasStatistics();
    Task<IEnumerable<AverageScoreByTypeDto>> GetaverageScoreByType();
    Task<ShelterPagedResponseDto> GetPagedShelters(int page = 1, int pageSize = 10);

}


   
