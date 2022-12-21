using Microsoft.EntityFrameworkCore;
using SampleIntegrationTest.SharedKernel.Application;
using SampleIntegrationTest.Domain.Meetings;
using SampleIntegrationTest.Infrastructure.Persistence;

namespace SampleIntegrationTest.Infrastructure.Domain.Meetings
{
    public class MeetingRepository : RepositoryBase<Meeting>, IMeetingRepository
    {
        public MeetingRepository(SampleDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Pagination<Meeting>> GetListByHostMsisdnAsync(long msisdn, int skip, int take, CancellationToken token)
        {
            var count = await base.DbContext.Set<Meeting>().CountAsync(c => c.HostMsisdn == msisdn);
            if (count == 0)
                return new Pagination<Meeting>()
                {
                    Items = new List<Meeting>(),
                    TotalItems = 0
                };

            return new Pagination<Meeting>()
            {
                Items = await base.DbContext.Set<Meeting>().Where(c => c.HostMsisdn == msisdn).Skip(take * skip).Take(take).ToListAsync(),
                TotalItems = count
            };
        }

        public void DetachEntity(Meeting meetingEntity)
        {
            DbContext.Entry(meetingEntity).State = EntityState.Detached;
        }

        public async Task<bool> AnyFreeTimeAsync(long msisdn, DateTimeOffset startDate)
        {
            return !await base.DbContext.Set<Meeting>().AnyAsync(a => a.HostMsisdn.Equals(msisdn) && a.StartDate <= startDate && a.EndDate >= startDate);
        }
    }
}
