using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.Infrastructure.Persistence.Configurations;

public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
{
    public void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        builder.ToTable("LeaveRequests", t =>
        {
            t.HasCheckConstraint("CK_LeaveRequests_DateRange", "[EndDate] >= [StartDate]");
            t.HasCheckConstraint("CK_LeaveRequests_Status", "[Status] IN ('Pending','Approved','Rejected')");
            t.HasCheckConstraint("CK_LeaveRequests_Type", "[Type] IN ('Vacation','Sick','Unpaid')");
            t.HasCheckConstraint("CK_LeaveRequests_EmployeeId", "[EmployeeId] > 0");
        });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StartDate).HasColumnType("date");
        builder.Property(x => x.EndDate).HasColumnType("date");

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetimeoffset(0)")
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        builder.Property(x => x.ReviewerNote)
            .HasMaxLength(500);

        builder.HasIndex(x => new { x.EmployeeId, x.Status });

        builder.HasIndex(x => x.StartDate);

        builder.HasIndex(x => new { x.EmployeeId, x.StartDate, x.EndDate })
            .IsUnique()
            .HasFilter("[Status] = 'Pending'");
    }
}
