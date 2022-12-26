using SampleIntegrationTest.SharedKernel.Application;
using SampleIntegrationTest.Api.Models.Meetings;
using SampleIntegrationTest.Tests.Setup;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using SampleIntegrationTest.Tests.Builders;

namespace SampleIntegrationTest.Tests.Meetings
{
    public class GetAllMeetingsTests : BaseIntegrationTest
    {
        private MeetingBuilder _meetingBuilder;
        [Fact]
        public async Task GetAllMeetingsAsync_Works_Correct()
        {
            await InitAsync(FakeApiType.Minimal);

            // Arrange
            var msisdn = 9165770705;
            var endDate = DateTimeOffset.Now.AddHours(1);
            var startDate = DateTimeOffset.Now;
            await _meetingBuilder.AddMeetingAsync(msisdn, startDate, endDate);

            // Act
            var response = await _httpClient.GetAsync($"/meeting/{msisdn}?Offset=0&Count=10");
            var retrievedMeetings = await response.Content.ReadFromJsonAsync<Pagination<MeetingViewModel>>();

            // Assert
            Assert.NotNull(retrievedMeetings);
            Assert.True(retrievedMeetings.Items.Any());
            Assert.Single(retrievedMeetings.Items);
            Assert.Contains(retrievedMeetings.Items, a => a.HostMsisdn == msisdn);
        }

        protected override async Task InitAsync(FakeApiType fakeApiType)
        {
            await base.InitAsync(fakeApiType);
            var scope = _fakeApi.Services.CreateScope();
            _meetingBuilder = scope.ServiceProvider.GetRequiredService<MeetingBuilder>();
        }
    }
}
