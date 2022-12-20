using SampleIntegrationTest.Domain.Meetings;

namespace SampleIntegrationTest.Api.Models.Meetings
{
    public class MeetingViewModel
    {
        public Guid Id { get; set; }
        public long HostMsisdn { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public MeetingStatus Status { get; set; }
    }
}
