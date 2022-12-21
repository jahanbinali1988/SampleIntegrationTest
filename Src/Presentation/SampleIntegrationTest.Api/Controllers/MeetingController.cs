using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SampleIntegrationTest.SharedKernel.Application;
using SampleIntegrationTest.Api.Models.Meetings;
using SampleIntegrationTest.Application.Contract.Meetings.Command;
using SampleIntegrationTest.Application.Contract.Meetings.Query;
using SampleIntegrationTest.Infrastructure.ThirdParty;

namespace SampleIntegrationTest.Api.Controllers
{
    [ApiController]
    [Route("meeting")]
    public class MeetingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IThirdPartyService _thirdPartyService;
        private readonly ILogger<MeetingController> _logger;

        public MeetingController(IMediator mediator, ILogger<MeetingController> logger, IThirdPartyService thirdPartyService)
        {
            _mediator = mediator;
            _logger = logger;
            _thirdPartyService = thirdPartyService;
        }

        [HttpPost]
        public async Task<ActionResult<MeetingViewModel>> CreateAsync([FromBody] CreateMeetingRequest request)
        {
            var command = request.Adapt<CreateMeetingCommand>();
            var meeting = await _mediator.Send(command);

            return meeting.Adapt<MeetingViewModel>();
        }


        [HttpGet("{msisdn:required}")]
        public async Task<ActionResult<Pagination<MeetingViewModel>>> GetMeetingListAsync([FromRoute] long msisdn, [FromQuery] GetListRequest request)
        {
            var query = new GetHostMeetingsListQuery(msisdn, request.Offset, request.Count);

            var meetings = await _mediator.Send(query);

            return meetings.Adapt<Pagination<MeetingViewModel>>();
        }


        [HttpGet("{id:guid}/details")]
        public async Task<ActionResult<MeetingDetailsViewModel>> GetMeetingListAsync([FromRoute] Guid id)
        {
            var details = await _thirdPartyService.GetMeetingDetailsAsync(id);
            return details.Adapt<MeetingDetailsViewModel>();
        }
    }
}