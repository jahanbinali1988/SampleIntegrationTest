namespace SampleIntegrationTest.Infrastructure.ThirdParty.Exceptions
{
    public class GetMeetingDetailsBusinessEception : Exception
    {
        public GetMeetingDetailsBusinessEception(Guid meetingId) : base($"There is some problem in retrieving meeting detail for given meeting id '{meetingId}'")
        {

        }
    }
}
