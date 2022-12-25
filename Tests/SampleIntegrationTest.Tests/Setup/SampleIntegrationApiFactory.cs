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
using SampleIntegrationTest.Domain.Meetings.DomainServices;
using SampleIntegrationTest.Infrastructure.Domain.Meetings;
using SampleIntegrationTest.Infrastructure.Persistence;
using SampleIntegrationTest.Tests.Builders;

namespace SampleIntegrationTest.Tests.Setup
{
    public class SampleIntegrationApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly TestcontainerDatabase _dbContainer;
        private readonly TestcontainersContainer _thirdPartyServiceContainer;

        private readonly ThirdPartyContainerBuilder _thirdPartyBuilder;
        private readonly EfContainerBuilder _efBuilder;
        public SampleIntegrationApiFactory()
        {
            _efBuilder = new EfContainerBuilder();
            _dbContainer = _efBuilder.Build();

            _thirdPartyBuilder = new ThirdPartyContainerBuilder();
            _thirdPartyServiceContainer = _thirdPartyBuilder.Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var configurations = new List<KeyValuePair<string, string>>();
            configurations.AddRange(_efBuilder.GetConfiguration());
            configurations.AddRange(_thirdPartyBuilder.GetConfiguration());

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
            await _dbContainer.StartAsync();
            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SampleDbContext>();
            dbContext.Database.EnsureCreated();

            await _thirdPartyServiceContainer.StartAsync().ConfigureAwait(true);
        }

        public async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync();

            await _thirdPartyServiceContainer.DisposeAsync();
        }
    }
}
