using DotNet.Testcontainers.Containers;

namespace SampleIntegrationTest.Tests.Fixtures
{
    public abstract class BaseDatabaseFixture<TDockerContainer, TDatabaseConnection> : IAsyncLifetime, IDisposable
        where TDockerContainer : ITestcontainersContainer
    {
        public TDockerContainer Container { get; protected set; }

        public TDatabaseConnection Connection { get; protected set; }

        public abstract Task InitializeAsync();

        public abstract Task DisposeAsync();

        public abstract void Dispose();

        public abstract KeyValuePair<string, string>[] GetConfiguration();
    }
}
