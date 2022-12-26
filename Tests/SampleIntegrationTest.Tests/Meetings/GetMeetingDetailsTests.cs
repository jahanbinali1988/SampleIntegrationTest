using Microsoft.Extensions.DependencyInjection;
using SampleIntegrationTest.Api.Models.Meetings;
using SampleIntegrationTest.Tests.Builders;
using SampleIntegrationTest.Tests.Setup;
using System.Net.Http.Json;

namespace SampleIntegrationTest.Tests.Meetings
{
    public class GetMeetingDetailsTests : IntegrationTestBase
    {
        private MeetingBuilder _meetingCreator;
        public GetMeetingDetailsTests() : base()
        {
        }

        [Fact]
        public async Task GetMeetingDetailsAsync_Works_Correct()
        {
            base._apiFactory = new SampleIntegrationApiFactory();
            await _apiFactory.InitializeAsync();
            var scope = base._apiFactory.Services.CreateScope();
            _meetingCreator = scope.ServiceProvider.GetRequiredService<MeetingBuilder>();

            // Arrange
            var msisdn = 9165770705;
            var endDate = DateTimeOffset.Now.AddHours(1);
            var startDate = DateTimeOffset.Now;
            var id = await _meetingCreator.AddMeetingAsync(msisdn, startDate, endDate);

            // Act
            HttpClient _client = _apiFactory.CreateClient();
            var response = await _client.GetAsync($"/meeting/{id}/details");
            var retrievedMeetings = await response.Content.ReadFromJsonAsync<MeetingDetailsViewModel>();

            // Assert
            Assert.NotNull(retrievedMeetings);
            Assert.Equal(id, retrievedMeetings.Id);
        }
    }
}
