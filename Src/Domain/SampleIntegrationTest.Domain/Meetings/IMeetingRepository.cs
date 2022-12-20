using Sample.SharedKernel.Application;
using Sample.SharedKernel.Shared;

namespace SampleIntegrationTest.Domain.Meetings
{
    public interface IMeetingRepository : IRepository<Meeting>
    {
        Task<bool> AnyFreeTimeAsync(long msisdn, DateTimeOffset startDate);
        void DetachEntity(Meeting meetingEntity);
        Task<Pagination<Meeting>> GetListByHostMsisdnAsync(long msisdn, int skip, int take, CancellationToken token);
    }
}
