using Microsoft.Extensions.DependencyInjection;
using SampleIntegrationTest.SharedKernel.Application;
using SampleIntegrationTest.Api.Models.Meetings;
using SampleIntegrationTest.Tests.Builders;
using SampleIntegrationTest.Tests.Setup;
using System.Net.Http.Json;

namespace SampleIntegrationTest.Tests.Meetings
{
    public class GetAllMeetingsTests
    {
        [Fact]
        public async Task GetAllMeetingsAsync_Works_Correct()
        {
            var _apiFactory = new SampleIntegrationApiFactory();
            await _apiFactory.InitializeAsync();
            var scope = _apiFactory.Services.CreateScope();
            var _meetingCreator = scope.ServiceProvider.GetRequiredService<MeetingBuilder>();

            // Arrange
            var msisdn = 9165770705;
            var endDate = DateTimeOffset.Now.AddHours(1);
            var startDate = DateTimeOffset.Now;
            await _meetingCreator.AddMeetingAsync(msisdn, startDate, endDate);

            // Act
            HttpClient _client = _apiFactory.CreateClient();
            var response = await _client.GetAsync($"/meeting/{msisdn}?Offset=0&Count=10");
            var retrievedMeetings = await response.Content.ReadFromJsonAsync<Pagination<MeetingViewModel>>();

            // Assert
            Assert.NotNull(retrievedMeetings);
            Assert.True(retrievedMeetings.Items.Any());
            Assert.Single(retrievedMeetings.Items);
            Assert.Contains(retrievedMeetings.Items, a => a.HostMsisdn == msisdn);
        }

    }
}
