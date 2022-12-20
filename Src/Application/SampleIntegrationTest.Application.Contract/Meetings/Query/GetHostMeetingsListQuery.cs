using Sample.SharedKernel.Application;
using SampleIntegrationTest.Application.Contract.Meetings.Dto;
using SampleIntegrationTest.Application.Contract.Shared;

namespace SampleIntegrationTest.Application.Contract.Meetings.Query
{
    public class GetHostMeetingsListQuery : GetListQueryBase, IQuery<Pagination<MeetingResponseDto>>
    {
        public GetHostMeetingsListQuery(long hostMsisdn, int offset, int count) : base(offset, count)
        {
            HostMsisdn = hostMsisdn;
        }

        public long HostMsisdn { get; private set; }
    }
}
