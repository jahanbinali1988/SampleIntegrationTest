using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SampleIntegrationTest.SharedKernel.Application;
using SampleIntegrationTest.Api.Models.Meetings;
using SampleIntegrationTest.Application.Contract.Meetings.Dto;
using SampleIntegrationTest.Tests.Builders;
using SampleIntegrationTest.Tests.Setup;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace SampleIntegrationTest.Tests.Meetings
{
    public class MeetingEndpointsTests : IntegrationTestBase
    {
        private MeetingBuilder _meetingCreator;
        public MeetingEndpointsTests() :base()
        {
        }

        [Fact]
        public async Task CreateMeetingsAsync_Works_Correct()
        {
            await _apiFactory.InitializeAsync();
            var scope = base._apiFactory.Services.CreateScope();
            _meetingCreator = scope.ServiceProvider.GetRequiredService<MeetingBuilder>();

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
            HttpClient _client = _apiFactory.CreateClient();
            var response = await _client.PostAsync($"meeting", httpContent);
            var retrievedMeeting = await response.Content.ReadFromJsonAsync<MeetingResponseDto>();

            // Assert
            Assert.NotNull(retrievedMeeting);
            Assert.Equal(msisdn, retrievedMeeting.HostMsisdn);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllMeetingsAsync_Works_Correct()
        {
            await _apiFactory.InitializeAsync();
            var scope = base._apiFactory.Services.CreateScope();
            _meetingCreator = scope.ServiceProvider.GetRequiredService<MeetingBuilder>();

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

        [Fact]
        public async Task GetMeetingDetailsAsync_Works_Correct()
        {
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
