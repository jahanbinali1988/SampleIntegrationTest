using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Logging;

namespace SampleIntegrationTest.Tests.Setup
{
    public sealed class AcceptsClientConnections : IWaitUntil
    {
        public async Task<bool> Until(ITestcontainersContainer testcontainers, ILogger logger)
        {
            var (stdout, _) = await testcontainers.GetLogs()
              .ConfigureAwait(false);
            return stdout.Contains("SQL Server is now ready for client connections.", StringComparison.OrdinalIgnoreCase);
        }
    }
}
