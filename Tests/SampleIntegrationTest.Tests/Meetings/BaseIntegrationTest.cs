using Microsoft.Extensions.DependencyInjection;
using SampleIntegrationTest.Tests.Builders;
using SampleIntegrationTest.Tests.Setup;

namespace SampleIntegrationTest.Tests.Meetings
{
    public abstract class BaseIntegrationTest 
    {
        protected BaseFakeApi _fakeApi;
        protected HttpClient _httpClient;
        protected MeetingBuilder _meetingBuilder;

        protected async Task Init(FakeApiType fakeApiType)
        {
            _fakeApi = FakeApiFactory.GetApi(fakeApiType);
            await _fakeApi.InitializeAsync();
            var scope = _fakeApi.Services.CreateScope();
            _meetingBuilder = scope.ServiceProvider.GetRequiredService<MeetingBuilder>();
            _httpClient = _fakeApi.CreateClient();
        }
    }
}
