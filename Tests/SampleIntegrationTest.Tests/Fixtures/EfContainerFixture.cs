using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace SampleIntegrationTest.Tests.Fixtures
{
    public sealed class EfContainerFixture : DatabaseFixture<MsSqlTestcontainer, DbConnection>
    {
        private readonly TestcontainerDatabaseConfiguration configuration = new MsSqlTestcontainerConfiguration
        {
            Database = $"TestDatabase{Guid.NewGuid()}",
            Password = ConstantVariables.SqlPassword
        };

        public EfContainerFixture()
        {
            Container = new TestcontainersBuilder<MsSqlTestcontainer>()
                    .WithDatabase(configuration)
                    .WithEnvironment("MSSQL_SA_PASSWORD ", ConstantVariables.SqlPassword)
                    .WithEnvironment("SA_PASSWORD ", ConstantVariables.SqlPassword)
                    .WithEnvironment("ACCEPT_EULA", "Y")
                    .WithEnvironment("TrustServerCertificate", "True")
                    .WithEnvironment("Trusted_Connection", "True")
                    .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                    .WithCleanUp(true)
                    .Build();
        }

        public override void Dispose()
        {
            configuration.Dispose();
        }

        public override async Task DisposeAsync()
        {
            Connection.Dispose();

            await Container.DisposeAsync()
              .ConfigureAwait(false);
        }

        public override async Task InitializeAsync()
        {
            await Container.StartAsync().ConfigureAwait(false);

            Connection = new SqlConnection(Container.ConnectionString);
        }

        public override KeyValuePair<string, string>[] GetConfiguration()
        {
            List<KeyValuePair<string, string>> configs = new List<KeyValuePair<string, string>>();

            var connectionString = $"{Container.ConnectionString};TrustServerCertificate=True;MultipleActiveResultSets=True";
            configs.Add(new KeyValuePair<string, string>("ConnectionStrings:DefaultConnection", connectionString));

            return configs.ToArray();
        }
    }
}
