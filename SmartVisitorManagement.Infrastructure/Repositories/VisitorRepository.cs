using Microsoft.EntityFrameworkCore;
using SmartVisitorManagement.Core.Common;
using SmartVisitorManagement.Core.Entities;
using SmartVisitorManagement.Core.Filters;
using SmartVisitorManagement.Core.Interfaces.Repositories;
using SmartVisitorManagement.Infrastructure.Data;

namespace SmartVisitorManagement.Infrastructure.Repositories;

public class VisitorRepository : BaseRepository<Visitor>, IVisitorRepository
{
    public VisitorRepository(AppDbContext context) : base(context) { }

    public async Task<PagedResult<Visitor>> GetAllAsync(VisitorSearchFilter filter)
    {
        var query = _dbSet.Include(v => v.Visits).AsQueryable();

        // Search filters
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(v => v.FullName.Contains(filter.Name));

        if (!string.IsNullOrWhiteSpace(filter.PhoneNumber))
            query = query.Where(v => v.PhoneNumber.Contains(filter.PhoneNumber));

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(v => v.CreatedAt)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PagedResult<Visitor>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
        };
    }

    public async Task<Visitor?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(v => v.Visits)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Visitor?> GetByPhoneAsync(string phone)
    {
        return await _dbSet.FirstOrDefaultAsync(v => v.PhoneNumber == phone);
    }

    public async Task<Visitor?> GetByNationalIdAsync(string nationalId)
    {
        return await _dbSet.FirstOrDefaultAsync(v => v.NationalId == nationalId);
    }

    public async Task<Visitor> CreateAsync(Visitor visitor)
    {
        await _dbSet.AddAsync(visitor);
        await SaveChangesAsync();
        return visitor;
    }

    public async Task<Visitor> UpdateAsync(Visitor visitor)
    {
        _dbSet.Update(visitor);
        await SaveChangesAsync();
        return visitor;
    }

    public async Task DeleteAsync(Visitor visitor)
    {
        _dbSet.Remove(visitor);
        await SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _dbSet.AnyAsync(v => v.Id == id);
    }
}