using Mapster;
using Sample.SharedKernel.Application;
using SampleIntegrationTest.Application.Contract.Meetings.Dto;
using SampleIntegrationTest.Application.Contract.Meetings.Query;
using SampleIntegrationTest.Domain.Meetings;

namespace SampleIntegrationTest.Application.Meetings.Query
{
    public class GetHostMeetingsListQueryHandler : IQueryHandler<GetHostMeetingsListQuery, Pagination<MeetingResponseDto>>
    {
        private readonly IMeetingRepository _repository;
        public GetHostMeetingsListQueryHandler(IMeetingRepository repository)
        {
            _repository = repository;
        }

        public async Task<Pagination<MeetingResponseDto>> Handle(GetHostMeetingsListQuery request, CancellationToken cancellationToken)
        {
            var meetings = await _repository.GetListByHostMsisdnAsync(request.HostMsisdn, request.Offset, request.Count, cancellationToken);

            return meetings.Adapt<Pagination<MeetingResponseDto>>();
        }
    }
}
