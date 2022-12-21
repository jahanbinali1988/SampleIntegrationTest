using SampleIntegrationTest.SharedKernel.SeedWork;
using SampleIntegrationTest.Domain.Meetings.DomainServices;
using SampleIntegrationTest.Domain.Meetings.Rules;

namespace SampleIntegrationTest.Domain.Meetings
{
    public class Meeting : Entity<Guid>, IAggregateRoot
    {
        public long HostMsisdn { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public static async Task<Meeting> CreateAsync(long hostMsisdn, DateTimeOffset startDate, DateTimeOffset endDate, ICheckUserFreeTimeService checkUserFreeTimeService)
        {
            await CheckRule(new UserShouldHaveFreeTimeRule(checkUserFreeTimeService, hostMsisdn, startDate));

            return new Meeting()
            {
                HostMsisdn = hostMsisdn,
                StartDate = startDate,
                EndDate = endDate,
                Id = Guid.NewGuid()
            };
        }
    }
}
