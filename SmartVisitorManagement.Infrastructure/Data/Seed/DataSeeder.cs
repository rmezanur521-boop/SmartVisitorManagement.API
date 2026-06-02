using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartVisitorManagement.Core.Common;
using SmartVisitorManagement.Core.Entities;
using SmartVisitorManagement.Infrastructure.Data;

namespace SmartVisitorManagement.Infrastructure.Data.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.MigrateAsync();

        if (await context.Visitors.AnyAsync()) return;

        var visitors = new List<Visitor>
        {
            new() { FullName = "Rahim Uddin",     PhoneNumber = "01711000001", Email = "rahim@example.com",   NationalId = "NID100001", Address = "Dhaka, Bangladesh",     CreatedAt = DateTime.UtcNow },
            new() { FullName = "Karim Hossain",   PhoneNumber = "01711000002", Email = "karim@example.com",   NationalId = "NID100002", Address = "Chittagong, Bangladesh", CreatedAt = DateTime.UtcNow },
            new() { FullName = "Sumaiya Begum",   PhoneNumber = "01711000003", Email = "sumaiya@example.com", NationalId = "NID100003", Address = "Sylhet, Bangladesh",     CreatedAt = DateTime.UtcNow },
            new() { FullName = "Tariqul Islam",   PhoneNumber = "01711000004", Email = "tariqul@example.com", NationalId = "NID100004", Address = "Rajshahi, Bangladesh",   CreatedAt = DateTime.UtcNow },
            new() { FullName = "Nasrin Akter",    PhoneNumber = "01711000005", Email = "nasrin@example.com",  NationalId = "NID100005", Address = "Khulna, Bangladesh",     CreatedAt = DateTime.UtcNow },
        };

        await context.Visitors.AddRangeAsync(visitors);
        await context.SaveChangesAsync();

        var visits = new List<Visit>
        {
            new() { VisitorId = visitors[0].Id, CheckInTime = DateTime.UtcNow.AddHours(-3), Purpose = "Business Meeting",    HostName = "Mr. Ahmed",  Status = VisitStatus.Completed, CheckOutTime = DateTime.UtcNow.AddHours(-1) },
            new() { VisitorId = visitors[1].Id, CheckInTime = DateTime.UtcNow.AddHours(-2), Purpose = "Job Interview",       HostName = "Ms. Fatema", Status = VisitStatus.Completed, CheckOutTime = DateTime.UtcNow.AddMinutes(-30) },
            new() { VisitorId = visitors[2].Id, CheckInTime = DateTime.UtcNow.AddHours(-1), Purpose = "Document Submission", HostName = "Mr. Hasan",  Status = VisitStatus.Active },
            new() { VisitorId = visitors[3].Id, CheckInTime = DateTime.UtcNow.AddMinutes(-45), Purpose = "Vendor Meeting",   HostName = "Ms. Ritu",   Status = VisitStatus.Active },
            new() { VisitorId = visitors[4].Id, CheckInTime = DateTime.UtcNow.AddMinutes(-20), Purpose = "Delivery",         HostName = "Mr. Kamal",  Status = VisitStatus.Active },
        };

        await context.Visits.AddRangeAsync(visits);
        await context.SaveChangesAsync();
    }
}