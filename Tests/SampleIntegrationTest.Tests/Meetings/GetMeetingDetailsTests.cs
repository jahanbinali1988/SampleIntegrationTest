using Microsoft.Extensions.DependencyInjection;
using SampleIntegrationTest.Api.Models.Meetings;
using SampleIntegrationTest.Tests.Builders;
using SampleIntegrationTest.Tests.Setup;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SampleIntegrationTest.Tests.Meetings
{
    public class GetMeetingDetailsTests : BaseIntegrationTest
    {
        private MeetingBuilder _meetingBuilder;

        [Fact]
        public async Task GetMeetingDetailsAsync_Works_Correct()
        {
            await InitAsync(FakeApiType.Maximal);

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

        protected override async Task InitAsync(FakeApiType fakeApiType)
        {
            await base.InitAsync(fakeApiType);
            var scope = _fakeApi.Services.CreateScope();
            _meetingBuilder = scope.ServiceProvider.GetRequiredService<MeetingBuilder>();
        }

    }
}
