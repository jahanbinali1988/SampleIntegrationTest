using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SampleIntegrationTest.Domain.Meetings.DomainServices;
using SampleIntegrationTest.Infrastructure.Domain.Meetings;
using SampleIntegrationTest.Infrastructure.Persistence;
using SampleIntegrationTest.Tests.Builders;
using System.ComponentModel;

namespace SampleIntegrationTest.Tests.Setup
{
    public class SampleIntegrationApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly TestcontainerDatabase _dbContainer;
        private int _thirdPartyFakePort;
        private readonly TestcontainersContainer _serviceContainer;
        public SampleIntegrationApiFactory()
        {
            _dbContainer = new TestcontainersBuilder<MsSqlTestcontainer>()
                .WithDatabase(new MsSqlTestcontainerConfiguration
                {
                    Database = "SampleIntegrationTestDb" + DateTime.Now.Ticks,
                    Password = "J@han14153",
                })
                .WithName("SampleIntegrationTestSql" + DateTime.Now.Ticks)
                .WithEnvironment("MSSQL_SA_PASSWORD ", "J@han14153")
                .WithEnvironment("SA_PASSWORD ", "J@han14153")
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithEnvironment("TrustServerCertificate", "True")
                .WithEnvironment("Trusted_Connection", "True")
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithCleanUp(true)
                .Build();

            GenerateRandomPort();
            _serviceContainer = new TestcontainersBuilder<TestcontainersContainer>()
                .WithImage("webapplication2:latest")
                .WithPortBinding(80, true)
                .WithExposedPort(80)
                .WithPortBinding(_thirdPartyFakePort, 80)
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var dbConnection = _dbContainer.ConnectionString + "TrustServerCertificate=True;MultipleActiveResultSets=True";
            var thirdPartyBaseUrl = $"http://{_serviceContainer.Hostname}:{_thirdPartyFakePort}"; //$"localhost:{_thirdPartyFakePort}";
            var thirdPartyServiceAddress = thirdPartyBaseUrl + "/meeting/{0}/details";
            IConfiguration config = new ConfigurationBuilder()
              .AddInMemoryCollection(new[]{
                  new KeyValuePair<string, string>("ConnectionStrings:DefaultConnection", dbConnection),
                  new KeyValuePair<string, string>("ThirdPartyConfiguration:GetMeetingDetailUrl", thirdPartyServiceAddress)
               }).Build();

            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(SampleDbContext));
                services.AddDbContext<SampleDbContext>(options =>
                {
                    options.UseSqlServer(_dbContainer.ConnectionString, x => x.MigrationsAssembly(typeof(MeetingTypeConfiguration).Assembly.ToString())
                );
                });
                services.AddTransient<MeetingBuilder>();
                services.AddTransient<ICheckUserFreeTimeService, CheckUserFreeTimeService>();
            }).UseConfiguration(config);
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SampleDbContext>();
            dbContext.Database.EnsureCreated();

            await _serviceContainer.StartAsync().ConfigureAwait(true);
        }

        public async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync();

            await _serviceContainer.DisposeAsync();
        }

        private int GenerateRandomPort()
        {
            if (_thirdPartyFakePort != default)
                return _thirdPartyFakePort;

            Random rnd = new Random();
            var port = rnd.Next(1000, 5000);
            _thirdPartyFakePort = port;
            return port;
        }
    }
}
