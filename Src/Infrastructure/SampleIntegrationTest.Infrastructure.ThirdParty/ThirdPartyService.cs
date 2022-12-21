using Microsoft.Extensions.Options;
using SampleIntegrationTest.Infrastructure.ThirdParty.Configurations;
using SampleIntegrationTest.Infrastructure.ThirdParty.Exceptions;
using SampleIntegrationTest.Infrastructure.ThirdParty.Extensions;
using SampleIntegrationTest.Infrastructure.ThirdParty.Models;

namespace SampleIntegrationTest.Infrastructure.ThirdParty
{
    public class ThirdPartyService : IThirdPartyService
    {
        private readonly HttpClient _httpClient;
        private readonly ThirdPartConfigurations _configurations;
        public ThirdPartyService(HttpClient httpClient, IOptions<ThirdPartConfigurations> thirdPartyConfiguration)
        {
            _httpClient = httpClient;
            _configurations = thirdPartyConfiguration.Value;
        }

        public async Task<MeetingDetailsResponseDto> GetMeetingDetailsAsync(Guid meetingId)
        {
            try
            {
                var response = await _httpClient.GetAsync(string.Format(_configurations.GetMeetingDetailUrl, meetingId));

                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                ApiResponse<MeetingDetailsResponseDto> result = await response.ReadContentAs<ApiResponse<MeetingDetailsResponseDto>>();

                return result.Data;
            }
            catch (Exception)
            {
                throw new GetMeetingDetailsBusinessEception(meetingId);
            }
        }
    }
}