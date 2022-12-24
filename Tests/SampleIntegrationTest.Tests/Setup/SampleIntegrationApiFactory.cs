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
        private readonly TestcontainersContainer _serviceContainer;
        public SampleIntegrationApiFactory()
        {
            _dbContainer = new TestcontainersBuilder<MsSqlTestcontainer>()
                .WithDatabase(new MsSqlTestcontainerConfiguration
                {
                    Database = "SampleIntegrationTestDb" + DateTime.Now.Ticks,
                    Password = "2@LaiNw)PDvs^t>L!Ybt]6H^%h3U>M",
                })
                .WithName("SampleIntegrationTestSql" + DateTime.Now.Ticks)
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithEnvironment("TrustServerCertificate", "True")
                .WithEnvironment("Trusted_Connection", "True")
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithCleanUp(true)
                .Build();

            _serviceContainer = new TestcontainersBuilder<TestcontainersContainer>()
                .WithImage("webapplication2:dev")
                .WithPortBinding(80, true)
                .WithExposedPort(80)
                .WithPortBinding(49163, 80)
                .WithEntrypoint("/bin/sh", "-c")
                .WithCommand($"while true; do echo \"$MAGIC_NUMBER\" | nc -l -p {80}; done")
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var dbConnection = _dbContainer.ConnectionString + "TrustServerCertificate=True;MultipleActiveResultSets=True";
            IConfiguration config = new ConfigurationBuilder()
              .AddInMemoryCollection(new[]{
                  new KeyValuePair<string, string>("ConnectionStrings:DefaultConnection", dbConnection),
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
            try
            {
                await _dbContainer.StartAsync();
                using var scope = Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<SampleDbContext>();
                dbContext.Database.EnsureCreated();

                await _serviceContainer.StartAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync();

            await _serviceContainer.DisposeAsync();
        }
    }
}
