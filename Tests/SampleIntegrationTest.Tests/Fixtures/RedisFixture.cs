using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using StackExchange.Redis;

namespace SampleIntegrationTest.Tests.Fixtures
{
    public sealed class RedisFixture : DatabaseFixture<RedisTestcontainer, IConnectionMultiplexer>
    {
        private readonly TestcontainerDatabaseConfiguration configuration = new RedisTestcontainerConfiguration();

        public RedisFixture()
        {
            this.Container = new TestcontainersBuilder<RedisTestcontainer>()
              .WithDatabase(this.configuration)
              .Build();
        }

        public override async Task InitializeAsync()
        {
            await this.Container.StartAsync()
              .ConfigureAwait(false);

            this.Connection = await ConnectionMultiplexer.ConnectAsync(this.Container.ConnectionString)
              .ConfigureAwait(false);
        }

        public override async Task DisposeAsync()
        {
            this.Connection.Dispose();

            await this.Container.DisposeAsync()
              .ConfigureAwait(false);
        }

        public override void Dispose()
        {
            this.configuration.Dispose();
        }

        public override KeyValuePair<string, string>[] GetConfiguration()
        {
            List<KeyValuePair<string, string>> configs = new List<KeyValuePair<string, string>>();

            configs.Add(new KeyValuePair<string, string>("ConnectionStrings:RedisDefaultConnection", Container.ConnectionString));

            return configs.ToArray();
        }
    }
}
