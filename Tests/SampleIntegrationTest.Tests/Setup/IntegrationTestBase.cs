namespace SampleIntegrationTest.Tests.Setup
{
    public class IntegrationTestBase 
    {
        protected readonly SampleIntegrationApiFactory _apiFactory;
        public IntegrationTestBase()
        {
            _apiFactory = new SampleIntegrationApiFactory();
        }
    }
}
