using Sample.SharedKernel.SeedWork;
using SampleIntegrationTest.Domain.Meetings.DomainServices;
using SampleIntegrationTest.Domain.Shared;

namespace SampleIntegrationTest.Domain.Meetings.Rules
{
    public class UserShouldHaveFreeTimeRule : IBusinessRule
    {
        private readonly ICheckUserFreeTimeService _checkUserFreeTimeService;
        private readonly long _msisdn;
        private readonly DateTimeOffset _startDate;
        public UserShouldHaveFreeTimeRule(ICheckUserFreeTimeService checkUserFreeTimeService, long msisdn, DateTimeOffset startDate)
        {
            _checkUserFreeTimeService = checkUserFreeTimeService;
            _msisdn = msisdn;
            _startDate = startDate;
        }

        public string Message => $"Unable to book meeting for given msisdn '{_msisdn}' at the given date '{_startDate}'";

        public string[] Properties => new[] { nameof(Meeting.HostMsisdn), nameof(Meeting.StartDate) };

        public string ErrorType => BusinessRuleType.Uniqueness.ToString("G");

        public async Task<bool> IsBroken() => !await _checkUserFreeTimeService.IsFree(_msisdn, _startDate);
    }
}
