namespace SampleIntegrationTest.Tests.Setup
{
    public class IntegrationTestBase : IClassFixture<SampleIntegrationApiFactory>
    {
        protected readonly HttpClient _client;
        public IntegrationTestBase(SampleIntegrationApiFactory apiFactory)
        {
            _client = apiFactory.CreateClient();
        }
    }
}
