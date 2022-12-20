namespace SampleIntegrationTest.Api.Models.Meetings
{
    public class CreateMeetingRequest
    {
        public long HostMsisdn { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
