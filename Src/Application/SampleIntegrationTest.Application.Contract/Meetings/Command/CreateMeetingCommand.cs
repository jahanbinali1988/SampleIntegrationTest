using SampleIntegrationTest.Application.Contract.Meetings.Dto;
using SampleIntegrationTest.SharedKernel.Application;

namespace SampleIntegrationTest.Application.Contract.Meetings.Command
{
    public class CreateMeetingCommand : CommandBase<MeetingResponseDto>
    {
        public CreateMeetingCommand()
        {

        }
        public CreateMeetingCommand(long hostMsisdn, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            HostMsisdn = hostMsisdn;
            StartDate = startDate;
            EndDate = endDate;
        }
        public long HostMsisdn { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
