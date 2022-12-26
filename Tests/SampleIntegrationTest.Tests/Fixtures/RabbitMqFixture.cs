using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using RabbitMQ.Client;

namespace SampleIntegrationTest.Tests.Fixtures
{
    public sealed class RabbitMqFixture : BaseDatabaseFixture<RabbitMqTestcontainer, IConnection>
    {
        private readonly TestcontainerMessageBrokerConfiguration configuration = new RabbitMqTestcontainerConfiguration { Username = "rabbitmq", Password = "rabbitmq" };

        public RabbitMqFixture()
        {
            Container = new TestcontainersBuilder<RabbitMqTestcontainer>()
              .WithMessageBroker(configuration)
              .Build();
        }

        public override async Task InitializeAsync()
        {
            await Container.StartAsync().ConfigureAwait(false);

            var connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri(Container.ConnectionString);
            Connection = connectionFactory.CreateConnection();
        }

        public override async Task DisposeAsync()
        {
            Connection.Dispose();

            await Container.DisposeAsync()
              .ConfigureAwait(false);
        }

        public override void Dispose()
        {
            configuration.Dispose();
        }

        public override KeyValuePair<string, string>[] GetConfiguration()
        {
            List<KeyValuePair<string, string>> configs = new List<KeyValuePair<string, string>>();

            configs.Add(new KeyValuePair<string, string>("RabbitMq:DefaultConnection", Connection.ToString()));

            return configs.ToArray();
        }
    }
}
