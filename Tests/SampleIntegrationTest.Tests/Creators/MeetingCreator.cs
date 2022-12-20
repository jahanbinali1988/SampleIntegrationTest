using Microsoft.Extensions.DependencyInjection;
using SampleIntegrationTest.Domain.Meetings;
using SampleIntegrationTest.Domain.Meetings.DomainServices;
using SampleIntegrationTest.Infrastructure.Persistence;

namespace SampleIntegrationTest.Tests.Creators
{
    public class MeetingCreator
    {
        private readonly SampleDbContext _dbContext;
        private readonly ICheckUserFreeTimeService _checkUserFreeTimeService;
        public MeetingCreator(ICheckUserFreeTimeService checkUserFreeTimeService, SampleDbContext dbContext)
        {
            this._checkUserFreeTimeService = checkUserFreeTimeService;
            _dbContext = dbContext;
        }

        public async Task<Guid> AddMeetingAsync(long msisdn, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var meeting = await Meeting.CreateAsync(msisdn, startDate, endDate, _checkUserFreeTimeService);
            
            await _dbContext.AddAsync(meeting);
            await _dbContext.SaveChangesAsync();

            return meeting.Id;
        }
    }
}
