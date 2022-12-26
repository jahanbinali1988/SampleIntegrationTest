using DotNet.Testcontainers.Containers;
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
using SampleIntegrationTest.Tests.Fixtures;

namespace SampleIntegrationTest.Tests.Setup
{
    public class SampleIntegrationApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly EfContainerFixture _databaseContainer;
        private readonly ThirdPartyContainerFixture _thirdPartyServiceContainer;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly RedisFixture _redisFixture;
        public SampleIntegrationApiFactory()
        {
            _databaseContainer = new EfContainerFixture();
            _thirdPartyServiceContainer = new ThirdPartyContainerFixture();
            _rabbitMqFixture = new RabbitMqFixture();
            _redisFixture = new RedisFixture();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var configurations = new List<KeyValuePair<string, string>>();
            configurations.AddRange(_databaseContainer.GetConfiguration());
            configurations.AddRange(_thirdPartyServiceContainer.GetConfiguration());
            configurations.AddRange(_rabbitMqFixture.GetConfiguration());
            configurations.AddRange(_redisFixture.GetConfiguration());

            IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(configurations).Build();

            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(SampleDbContext));
                services.AddDbContext<SampleDbContext>(options =>
                {
                    options.UseSqlServer(configurations.First(c=> c.Key == "ConnectionStrings:DefaultConnection").Value, x => x.MigrationsAssembly(typeof(MeetingTypeConfiguration).Assembly.ToString())
                );
                }, ServiceLifetime.Scoped);
                services.AddScoped<MeetingBuilder>();
                services.AddScoped<ICheckUserFreeTimeService, CheckUserFreeTimeService>();
            }).UseConfiguration(config);
        }

        public async Task InitializeAsync()
        {
            await _databaseContainer.InitializeAsync();
            await _thirdPartyServiceContainer.InitializeAsync().ConfigureAwait(true);
            await _rabbitMqFixture.InitializeAsync().ConfigureAwait(true);
            await _redisFixture.InitializeAsync().ConfigureAwait(true);

            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SampleDbContext>();
            dbContext.Database.EnsureCreated();
        }

        public async Task DisposeAsync()
        {
            await _databaseContainer.DisposeAsync();
            await _thirdPartyServiceContainer.DisposeAsync();
            await _rabbitMqFixture.DisposeAsync();
            await _redisFixture.DisposeAsync();

            GC.SuppressFinalize(this);
        }
    }
}
