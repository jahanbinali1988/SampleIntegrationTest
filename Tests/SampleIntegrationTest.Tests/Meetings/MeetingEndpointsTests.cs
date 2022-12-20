using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Sample.SharedKernel.Application;
using SampleIntegrationTest.Api.Models.Meetings;
using SampleIntegrationTest.Application.Contract.Meetings.Dto;
using SampleIntegrationTest.Infrastructure.Persistence;
using SampleIntegrationTest.Tests.Creators;
using SampleIntegrationTest.Tests.Setup;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace SampleIntegrationTest.Tests.Meetings
{
    public class MeetingEndpointsTests : IntegrationTestBase
    {
        private readonly MeetingCreator _meetingCreator;
        public MeetingEndpointsTests(InventoryApiFactory apiFactory) : base(apiFactory)
        {
            var scope = apiFactory.Services.CreateScope();
            _meetingCreator = scope.ServiceProvider.GetRequiredService<MeetingCreator>();
        }

        [Fact]
        public async Task CreateMeetingsAsync_Works_Correct()
        {
            // Arrange
            var msisdn = 9224957626;
            var request = new CreateMeetingRequest()
            {
                EndDate = DateTimeOffset.Now.AddDays(1),
                StartDate = DateTimeOffset.Now,
                HostMsisdn = msisdn
            };
            var serializeRequest = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(serializeRequest, UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await base._client.PostAsync($"meeting", httpContent);
            var retrievedMeeting = await response.Content.ReadFromJsonAsync<MeetingResponseDto>();

            // Assert
            Assert.NotNull(retrievedMeeting);
            Assert.Equal(msisdn, retrievedMeeting.HostMsisdn);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllMeetingsAsync_Works_Correct()
        {
            // Arrange
            var msisdn = 9165770705;
            var endDate = DateTimeOffset.Now.AddHours(1);
            var startDate = DateTimeOffset.Now;
            await _meetingCreator.AddMeetingAsync(msisdn, startDate, endDate);

            // Act
            var response = await base._client.GetAsync($"/meeting/{msisdn}/meetings?Offset=0&Count=10");
            var retrievedMeetings = await response.Content.ReadFromJsonAsync<Pagination<MeetingViewModel>>();

            // Assert
            Assert.NotNull(retrievedMeetings);
            Assert.True(retrievedMeetings.Items.Any());
            Assert.Equal(1, retrievedMeetings.Items.Count);
            Assert.Contains(retrievedMeetings.Items, a => a.HostMsisdn == msisdn);
        }
    }
}
