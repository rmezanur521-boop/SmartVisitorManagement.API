using SmartVisitorManagement.Core.Common;
using SmartVisitorManagement.Core.Entities;
using SmartVisitorManagement.Core.Filters;

namespace SmartVisitorManagement.Core.Interfaces.Repositories;

public interface IVisitorRepository
{
    Task<PagedResult<Visitor>> GetAllAsync(VisitorSearchFilter filter);
    Task<Visitor?> GetByIdAsync(int id);
    Task<Visitor?> GetByPhoneAsync(string phone);
    Task<Visitor?> GetByNationalIdAsync(string nationalId);
    Task<Visitor> CreateAsync(Visitor visitor);
    Task<Visitor> UpdateAsync(Visitor visitor);
    Task DeleteAsync(Visitor visitor);
    Task<bool> ExistsAsync(int id);
}