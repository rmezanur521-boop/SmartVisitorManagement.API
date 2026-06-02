using SmartVisitorManagement.Core.Common;
using SmartVisitorManagement.Core.DTOs.Visitor;
using SmartVisitorManagement.Core.Entities;
using SmartVisitorManagement.Core.Filters;
using SmartVisitorManagement.Core.Interfaces.Repositories;
using SmartVisitorManagement.Core.Interfaces.Services;

namespace SmartVisitorManagement.Services;

public class VisitorService : IVisitorService
{
    private readonly IVisitorRepository _visitorRepository;

    public VisitorService(IVisitorRepository visitorRepository)
    {
        _visitorRepository = visitorRepository;
    }

    public async Task<PagedResult<VisitorResponseDto>> GetAllAsync(VisitorSearchFilter filter)
    {
        var result = await _visitorRepository.GetAllAsync(filter);

        return new PagedResult<VisitorResponseDto>
        {
            Items = result.Items.Select(MapToDto).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }

    public async Task<VisitorResponseDto?> GetByIdAsync(int id)
    {
        var visitor = await _visitorRepository.GetByIdAsync(id);
        return visitor is null ? null : MapToDto(visitor);
    }

    public async Task<VisitorResponseDto> CreateAsync(CreateVisitorDto dto)
    {
        // Business rule: no duplicate phone or national ID
        var existingPhone = await _visitorRepository.GetByPhoneAsync(dto.PhoneNumber);
        if (existingPhone is not null)
            throw new InvalidOperationException($"A visitor with phone number '{dto.PhoneNumber}' already exists.");

        var existingNid = await _visitorRepository.GetByNationalIdAsync(dto.NationalId);
        if (existingNid is not null)
            throw new InvalidOperationException($"A visitor with National ID '{dto.NationalId}' already exists.");

        var visitor = new Visitor
        {
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            NationalId = dto.NationalId,
            Address = dto.Address,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _visitorRepository.CreateAsync(visitor);
        return MapToDto(created);
    }

    public async Task<VisitorResponseDto?> UpdateAsync(int id, UpdateVisitorDto dto)
    {
        var visitor = await _visitorRepository.GetByIdAsync(id);
        if (visitor is null) return null;

        visitor.FullName = dto.FullName;
        visitor.PhoneNumber = dto.PhoneNumber;
        visitor.Email = dto.Email;
        visitor.Address = dto.Address;

        var updated = await _visitorRepository.UpdateAsync(visitor);
        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var visitor = await _visitorRepository.GetByIdAsync(id);
        if (visitor is null) return false;

        await _visitorRepository.DeleteAsync(visitor);
        return true;
    }

    // ── Private mapper ──────────────────────────────────────────────────────
    private static VisitorResponseDto MapToDto(Visitor v) => new()
    {
        Id = v.Id,
        FullName = v.FullName,
        PhoneNumber = v.PhoneNumber,
        Email = v.Email,
        NationalId = v.NationalId,
        Address = v.Address,
        CreatedAt = v.CreatedAt,
        TotalVisits = v.Visits?.Count ?? 0
    };
}