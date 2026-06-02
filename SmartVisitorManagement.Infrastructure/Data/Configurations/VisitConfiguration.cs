using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartVisitorManagement.Core.Entities;

namespace SmartVisitorManagement.Infrastructure.Data.Configurations;

public class VisitConfiguration : IEntityTypeConfiguration<Visit>
{
    public void Configure(EntityTypeBuilder<Visit> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.CheckInTime)
            .IsRequired();

        builder.Property(v => v.CheckOutTime)
            .IsRequired(false);

        builder.Property(v => v.Purpose)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(v => v.HostName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.Status)
            .IsRequired()
            .HasConversion<int>();

        // Index for fast active-visit queries
        builder.HasIndex(v => v.Status);
        builder.HasIndex(v => v.CheckInTime);

        builder.ToTable("Visits");
    }
}