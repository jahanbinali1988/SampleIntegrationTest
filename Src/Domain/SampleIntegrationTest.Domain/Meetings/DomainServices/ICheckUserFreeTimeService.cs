namespace SampleIntegrationTest.Domain.Meetings.DomainServices
{
    public interface ICheckUserFreeTimeService
    {
        Task<bool> IsFree(long msisdn, DateTimeOffset startDate);
    }
}
