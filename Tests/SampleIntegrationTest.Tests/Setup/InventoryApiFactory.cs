using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SampleIntegrationTest.Api;
using SampleIntegrationTest.Domain.Meetings.DomainServices;
using SampleIntegrationTest.Infrastructure.Domain.Meetings;
using SampleIntegrationTest.Infrastructure.Persistence;
using SampleIntegrationTest.Tests.Builders;

namespace SampleIntegrationTest.Tests.Setup
{
    public class InventoryApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly TestcontainerDatabase _dbContainer;

        public InventoryApiFactory()
        {
            _dbContainer = new TestcontainersBuilder<MsSqlTestcontainer>()
                .WithDatabase(new MsSqlTestcontainerConfiguration
                {
                    Database = "WeatherApp",
                    Password = "2@LaiNw)PDvs^t>L!Ybt]6H^%h3U>M",
                })
                .WithName("SqlDb")
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithEnvironment("TrustServerCertificate", "True")
                .WithEnvironment("Trusted_Connection", "True")
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithCleanUp(true)
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
            await _dbContainer.StartAsync();
            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SampleDbContext>();
            dbContext.Database.EnsureCreated();
        }

        public new async Task DisposeAsync() => await _dbContainer.DisposeAsync();
    }
}
