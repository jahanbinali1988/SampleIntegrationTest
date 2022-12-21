using MediatR;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;
using SampleIntegrationTest.SharedKernel.EventProcessing.DomainEvent;
using SampleIntegrationTest.SharedKernel.SeedWork;
using SampleIntegrationTest.Application;
using SampleIntegrationTest.Domain.Meetings;
using SampleIntegrationTest.Domain.Meetings.DomainServices;
using SampleIntegrationTest.Infrastructure.Domain.Meetings;
using SampleIntegrationTest.Infrastructure.EventProcessing;
using SampleIntegrationTest.Infrastructure.Persistence;
using SampleIntegrationTest.Infrastructure.ThirdParty;
using SampleIntegrationTest.Infrastructure.ThirdParty.Configurations;
using SampleIntegrationTest.Infrastructure.ThirdParty.Extensions;

namespace SampleIntegrationTest.Api.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDbContext<SampleDbContext>(optrions => optrions.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddMediatR(typeof(Program).Assembly, typeof(AssembelyMarker).Assembly);
            services.AddTransient<IMeetingRepository, MeetingRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();

            services.AddTransient<ICheckUserFreeTimeService, CheckUserFreeTimeService>();


            ThirdPartConfigurations thirdPartyConfiguration = new();
            configuration.GetSection("ThirdPartyConfiguration").Bind(thirdPartyConfiguration);
            services.Configure<ThirdPartConfigurations>(configuration.GetSection("ThirdPartyConfiguration"));

            services.AddThirdPartyServices();
            services.AddHttpClientServices(configuration);

            return services;
        }

        public static IServiceCollection AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHttpClient<IThirdPartyService, ThirdPartyService>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
              .HandleTransientHttpError()
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
              .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}
