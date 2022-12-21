using SampleIntegrationTest.Infrastructure.ThirdParty.Models;

namespace SampleIntegrationTest.Infrastructure.ThirdParty
{
    public interface IThirdPartyService
    {
        Task<MeetingDetailsResponseDto> GetMeetingDetailsAsync(Guid meetingId);
    }
}