using SampleIntegrationTest.Api.Models.Meetings;
using SampleIntegrationTest.Tests.Setup;
using System.Net.Http.Json;

namespace SampleIntegrationTest.Tests.Meetings
{
    public class GetMeetingDetailsTests : BaseIntegrationTest
    {
        [Fact]
        public async Task GetMeetingDetailsAsync_Works_Correct()
        {
            await Init(FakeApiType.Maximal);

            // Arrange
            var msisdn = 9165770705;
            var endDate = DateTimeOffset.Now.AddHours(1);
            var startDate = DateTimeOffset.Now;
            var id = await _meetingBuilder.AddMeetingAsync(msisdn, startDate, endDate);

            // Act
            var response = await _httpClient.GetAsync($"/meeting/{id}/details");
            var retrievedMeetings = await response.Content.ReadFromJsonAsync<MeetingDetailsViewModel>();

            // Assert
            Assert.NotNull(retrievedMeetings);
            Assert.Equal(id, retrievedMeetings.Id);
        }
    }
}
