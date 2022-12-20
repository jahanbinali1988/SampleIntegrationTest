namespace SampleIntegrationTest.Domain.Meetings.DomainServices
{
    public class CheckUserFreeTimeService : ICheckUserFreeTimeService
    {
        private readonly IMeetingRepository _repository;
        public CheckUserFreeTimeService(IMeetingRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsFree(long msisdn, DateTimeOffset startDate)
        {
            return await _repository.AnyFreeTimeAsync(msisdn, startDate);
        }
    }
}
