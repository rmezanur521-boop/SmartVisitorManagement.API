using Microsoft.EntityFrameworkCore;
using SmartVisitorManagement.Infrastructure.Data;

namespace SmartVisitorManagement.Infrastructure.Repositories;

public abstract class BaseRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected BaseRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    protected async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}