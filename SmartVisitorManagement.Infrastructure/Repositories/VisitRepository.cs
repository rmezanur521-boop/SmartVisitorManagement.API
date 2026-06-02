using Microsoft.EntityFrameworkCore;
using SmartVisitorManagement.Core.Common;
using SmartVisitorManagement.Core.Entities;
using SmartVisitorManagement.Core.Filters;
using SmartVisitorManagement.Core.Interfaces.Repositories;
using SmartVisitorManagement.Infrastructure.Data;

namespace SmartVisitorManagement.Infrastructure.Repositories;

public class VisitRepository : BaseRepository<Visit>, IVisitRepository
{
    public VisitRepository(AppDbContext context) : base(context) { }

    public async Task<PagedResult<Visit>> GetHistoryAsync(VisitSearchFilter filter)
    {
        var query = _dbSet
            .Include(v => v.Visitor)
            .AsQueryable();

        if (filter.Date.HasValue)
        {
            var date = filter.Date.Value.Date;
            query = query.Where(v => v.CheckInTime.Date == date);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(v => v.CheckInTime)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PagedResult<Visit>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
        };
    }

    public async Task<List<Visit>> GetActiveVisitsAsync()
    {
        return await _dbSet
            .Include(v => v.Visitor)
            .Where(v => v.Status == VisitStatus.Active)
            .OrderByDescending(v => v.CheckInTime)
            .ToListAsync();
    }

    public async Task<Visit?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(v => v.Visitor)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Visit?> GetActiveVisitByVisitorIdAsync(int visitorId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(v => v.VisitorId == visitorId
                                   && v.Status == VisitStatus.Active);
    }

    public async Task<Visit> CreateAsync(Visit visit)
    {
        await _dbSet.AddAsync(visit);
        await SaveChangesAsync();
        return visit;
    }

    public async Task<Visit> UpdateAsync(Visit visit)
    {
        _dbSet.Update(visit);
        await SaveChangesAsync();
        return visit;
    }

    public async Task<List<Visit>> GetVisitsByDateAsync(DateTime date)
    {
        return await _dbSet
            .Include(v => v.Visitor)
            .Where(v => v.CheckInTime.Date == date.Date)
            .OrderByDescending(v => v.CheckInTime)
            .ToListAsync();
    }

    public async Task<int> GetActiveVisitCountAsync()
    {
        return await _dbSet.CountAsync(v => v.Status == VisitStatus.Active);
    }
}