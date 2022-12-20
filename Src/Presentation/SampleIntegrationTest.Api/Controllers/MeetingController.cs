using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sample.SharedKernel.Application;
using SampleIntegrationTest.Api.Models.Meetings;
using SampleIntegrationTest.Application.Contract.Meetings.Command;
using SampleIntegrationTest.Application.Contract.Meetings.Dto;
using SampleIntegrationTest.Application.Contract.Meetings.Query;

namespace SampleIntegrationTest.Api.Controllers
{
    [ApiController]
    [Route("meeting")]
    public class MeetingController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly ILogger<MeetingController> _logger;

        public MeetingController(IMediator mediator, ILogger<MeetingController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<MeetingViewModel>> CreateAsync([FromBody] CreateMeetingRequest request)
        {
            var command = request.Adapt<CreateMeetingCommand>();
            var meeting = await _mediator.Send(command);

            return meeting.Adapt<MeetingViewModel>();
        }


        [HttpGet("{msisdn:required}/meetings")]
        public async Task<ActionResult<Pagination<MeetingViewModel>>> GetMeetingListAsync([FromRoute] long msisdn, [FromQuery] GetListRequest request)
        {
            var query = new GetHostMeetingsListQuery(msisdn, request.Offset, request.Count);

            var meetings = await _mediator.Send(query, HttpContext.RequestAborted);

            return meetings.Adapt<Pagination<MeetingViewModel>>();
        }
    }
}