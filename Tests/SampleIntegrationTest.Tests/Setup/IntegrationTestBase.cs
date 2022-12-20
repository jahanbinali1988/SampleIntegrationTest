namespace SampleIntegrationTest.Tests.Setup
{
    public class IntegrationTestBase : IClassFixture<InventoryApiFactory>
    {
        protected readonly HttpClient _client;
        public IntegrationTestBase(InventoryApiFactory apiFactory)
        {
            _client = apiFactory.CreateClient();
        }
    }
}
