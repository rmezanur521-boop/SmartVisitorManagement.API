using Microsoft.EntityFrameworkCore;
using SmartVisitorManagement.Core.Entities;
using SmartVisitorManagement.Infrastructure.Data.Configurations;

namespace SmartVisitorManagement.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Visitor> Visitors => Set<Visitor>();
    public DbSet<Visit> Visits => Set<Visit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new VisitorConfiguration());
        modelBuilder.ApplyConfiguration(new VisitConfiguration());
    }
}