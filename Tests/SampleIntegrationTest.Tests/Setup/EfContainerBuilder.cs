using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;

namespace SampleIntegrationTest.Tests.Setup
{
    public class EfContainerBuilder
    {
        private static MsSqlTestcontainer? _dbcontainer;
        private readonly string _databaseName = $"ContantVariables.SqlDatabase{Guid.NewGuid()}";

        public TestcontainerDatabase Build()
        {
            var dbContainer = new TestcontainersBuilder<MsSqlTestcontainer>()
                    .WithDatabase(new MsSqlTestcontainerConfiguration
                    {
                        Database = _databaseName,
                        Password = ConstantVariables.SqlPassword,
                    })
                    .WithName(_databaseName)
                    .WithEnvironment("MSSQL_SA_PASSWORD ", ConstantVariables.SqlPassword)
                    .WithEnvironment("SA_PASSWORD ", ConstantVariables.SqlPassword)
                    .WithEnvironment("ACCEPT_EULA", "Y")
                    .WithEnvironment("TrustServerCertificate", "True")
                    .WithEnvironment("Trusted_Connection", "True")
                    .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                    .WithCleanUp(true)
                    .Build();

            _dbcontainer = dbContainer;
            return dbContainer;
        }

        public KeyValuePair<string, string>[] GetConfiguration()
        {
            if (_dbcontainer == null)
                Build();

            List<KeyValuePair<string, string>> configs = new List<KeyValuePair<string, string>>();

            var connectionString = $"{_dbcontainer.ConnectionString};TrustServerCertificate=True;MultipleActiveResultSets=True";
            configs.Add(new KeyValuePair<string, string>("ConnectionStrings:DefaultConnection", connectionString));

            return configs.ToArray();
        }
    }
}
