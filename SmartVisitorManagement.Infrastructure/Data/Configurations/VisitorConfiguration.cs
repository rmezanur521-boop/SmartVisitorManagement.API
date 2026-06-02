using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartVisitorManagement.Core.Entities;

namespace SmartVisitorManagement.Infrastructure.Data.Configurations;

public class VisitorConfiguration : IEntityTypeConfiguration<Visitor>
{
    public void Configure(EntityTypeBuilder<Visitor> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(v => v.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.NationalId)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(v => v.Address)
            .HasMaxLength(250);

        builder.Property(v => v.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        // Unique constraints
        builder.HasIndex(v => v.Email).IsUnique();
        builder.HasIndex(v => v.NationalId).IsUnique();
        builder.HasIndex(v => v.PhoneNumber).IsUnique();

        // Relationship
        builder.HasMany(v => v.Visits)
            .WithOne(vs => vs.Visitor)
            .HasForeignKey(vs => vs.VisitorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Visitors");
    }
}