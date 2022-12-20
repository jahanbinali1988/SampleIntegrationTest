using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Sample.SharedKernel.EventProcessing.DomainEvent;
using Sample.SharedKernel.SeedWork;
using SampleIntegrationTest.Application;
using SampleIntegrationTest.Domain.Meetings;
using SampleIntegrationTest.Domain.Meetings.DomainServices;
using SampleIntegrationTest.Infrastructure.Domain.Meetings;
using SampleIntegrationTest.Infrastructure.EventProcessing;
using SampleIntegrationTest.Infrastructure.Persistence;

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

            return services;
        }
    }
}
