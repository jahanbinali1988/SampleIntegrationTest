using Microsoft.Extensions.DependencyInjection;

namespace SampleIntegrationTest.Infrastructure.ThirdParty.Extensions
{
    public static class AddThirdPartyServiceCollectionExtensions
    {
        public static void AddThirdPartyServices(this IServiceCollection services)
        {
            services.AddTransient<IThirdPartyService, ThirdPartyService>();
        }
    }
}
