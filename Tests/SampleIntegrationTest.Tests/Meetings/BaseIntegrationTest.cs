using SampleIntegrationTest.Tests.Setup;

namespace SampleIntegrationTest.Tests.Meetings
{
    public abstract class BaseIntegrationTest 
    {
        protected BaseFakeApi _fakeApi;
        protected HttpClient _httpClient;

        protected virtual async Task InitAsync(FakeApiType fakeApiType) 
        {
            _fakeApi = FakeApiFactory.GetApi(fakeApiType);
            await _fakeApi.InitializeAsync();
            _httpClient = _fakeApi.CreateClient();
        }
    }
}
