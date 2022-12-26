using Microsoft.AspNetCore.Hosting;
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
    public class MinimalFakeApiBuilder : BaseFakeApi
    {
        private readonly EfContainerFixture _databaseContainer;
        public MinimalFakeApiBuilder()
        {
            _databaseContainer = new EfContainerFixture();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var configurations = new List<KeyValuePair<string, string>>();
            configurations.AddRange(_databaseContainer.GetConfiguration());

            IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(configurations).Build();

            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(SampleDbContext));
                services.AddDbContext<SampleDbContext>(options =>
                {
                    options.UseSqlServer(configurations.First(c => c.Key == "ConnectionStrings:DefaultConnection").Value, x => x.MigrationsAssembly(typeof(MeetingTypeConfiguration).Assembly.ToString())
                );
                }, ServiceLifetime.Scoped);
                services.AddScoped<MeetingBuilder>();
                services.AddScoped<ICheckUserFreeTimeService, CheckUserFreeTimeService>();
            }).UseConfiguration(config);
        }

        public override async Task InitializeAsync()
        {
            await _databaseContainer.InitializeAsync();

            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SampleDbContext>();
            dbContext.Database.EnsureCreated();
        }

        public override async Task DisposeAsync()
        {
            await _databaseContainer.DisposeAsync();
            GC.SuppressFinalize(this);
        }

    }
}
