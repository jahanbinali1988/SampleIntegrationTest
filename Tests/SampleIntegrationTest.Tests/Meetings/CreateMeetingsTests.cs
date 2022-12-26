using Newtonsoft.Json;
using SampleIntegrationTest.Api.Models.Meetings;
using SampleIntegrationTest.Application.Contract.Meetings.Dto;
using SampleIntegrationTest.Tests.Setup;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace SampleIntegrationTest.Tests.Meetings
{
    public class CreateMeetingsTests : BaseIntegrationTest
    {
        [Fact]
        public async Task CreateMeetingsAsync_Works_Correct()
        {
            await Init(FakeApiType.Maximal);

            // Arrange
            var msisdn = 9165770705;
            var request = new CreateMeetingRequest()
            {
                EndDate = DateTimeOffset.Now.AddDays(1),
                StartDate = DateTimeOffset.Now,
                HostMsisdn = msisdn
            };
            var serializeRequest = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(serializeRequest, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.PostAsync($"meeting", httpContent);
            var retrievedMeeting = await response.Content.ReadFromJsonAsync<MeetingResponseDto>();

            // Assert
            Assert.NotNull(retrievedMeeting);
            Assert.Equal(msisdn, retrievedMeeting.HostMsisdn);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
