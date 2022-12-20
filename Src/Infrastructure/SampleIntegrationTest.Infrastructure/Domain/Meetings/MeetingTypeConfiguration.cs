using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SampleIntegrationTest.Domain.Meetings;

namespace SampleIntegrationTest.Infrastructure.Domain.Meetings
{
    public class MeetingTypeConfiguration : IEntityTypeConfiguration<Meeting>
    {
        public void Configure(EntityTypeBuilder<Meeting> builder)
        {
            builder.HasIndex(x => x.Id).IsUnique();
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
            builder.Property<bool>("IsDeleted").HasDefaultValue(false);
            builder.Property<DateTimeOffset?>("DeletedAt").IsRequired(false);
            builder.HasQueryFilter(p => EF.Property<bool>(p, "IsDeleted") == false);

            builder.ToTable<Meeting>(nameof(Meeting));
        }
    }
}
