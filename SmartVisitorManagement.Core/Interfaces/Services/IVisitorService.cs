using SmartVisitorManagement.Core.Common;
using SmartVisitorManagement.Core.DTOs.Visitor;
using SmartVisitorManagement.Core.Filters;

namespace SmartVisitorManagement.Core.Interfaces.Services;

public interface IVisitorService
{
    Task<PagedResult<VisitorResponseDto>> GetAllAsync(VisitorSearchFilter filter);
    Task<VisitorResponseDto?> GetByIdAsync(int id);
    Task<VisitorResponseDto> CreateAsync(CreateVisitorDto dto);
    Task<VisitorResponseDto?> UpdateAsync(int id, UpdateVisitorDto dto);
    Task<bool> DeleteAsync(int id);
}