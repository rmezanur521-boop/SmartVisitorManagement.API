using SmartVisitorManagement.Core.Common;
using SmartVisitorManagement.Core.Entities;
using SmartVisitorManagement.Core.Filters;

namespace SmartVisitorManagement.Core.Interfaces.Repositories;

public interface IVisitRepository
{
    Task<PagedResult<Visit>> GetHistoryAsync(VisitSearchFilter filter);
    Task<List<Visit>> GetActiveVisitsAsync();
    Task<Visit?> GetByIdAsync(int id);
    Task<Visit?> GetActiveVisitByVisitorIdAsync(int visitorId);
    Task<Visit> CreateAsync(Visit visit);
    Task<Visit> UpdateAsync(Visit visit);
    Task<List<Visit>> GetVisitsByDateAsync(DateTime date);
    Task<int> GetActiveVisitCountAsync();
}