using Microsoft.AspNetCore.Mvc.Testing;

namespace SampleIntegrationTest.Tests.Setup
{
    public abstract class BaseFakeApi : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public abstract Task InitializeAsync();

        public abstract Task DisposeAsync();
    }
}
