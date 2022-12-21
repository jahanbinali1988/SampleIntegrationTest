using Mapster;
using SampleIntegrationTest.SharedKernel.Application;
using SampleIntegrationTest.SharedKernel.SeedWork;
using SampleIntegrationTest.Application.Contract.Meetings.Command;
using SampleIntegrationTest.Application.Contract.Meetings.Dto;
using SampleIntegrationTest.Domain.Meetings;
using SampleIntegrationTest.Domain.Meetings.DomainServices;

namespace SampleIntegrationTest.Application.Meetings.Command
{
    internal class CreateMeetingCommandHandler : ICommandHandler<CreateMeetingCommand, MeetingResponseDto>
    {
        private readonly IMeetingRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICheckUserFreeTimeService _checkUserFreeTimeService;
        public CreateMeetingCommandHandler(IMeetingRepository repository, IUnitOfWork unitOfWork, ICheckUserFreeTimeService checkUserFreeTimeService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _checkUserFreeTimeService = checkUserFreeTimeService;
        }

        public async Task<MeetingResponseDto> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
        {
            var meeting = await Meeting.CreateAsync(request.HostMsisdn, request.StartDate, request.EndDate, _checkUserFreeTimeService);

            await _repository.AddAsync(meeting, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            _repository.DetachEntity(meeting);

            return meeting.Adapt<MeetingResponseDto>();
        }
    }
}
