using Microsoft.EntityFrameworkCore;
using SampleIntegrationTest.Domain.Meetings;
using SampleIntegrationTest.Infrastructure.Domain.Meetings;
using System.Reflection;

namespace SampleIntegrationTest.Infrastructure.Persistence
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new MeetingTypeConfiguration());
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }

        public DbSet<Meeting> Meetings { get; set; }

    }
}
