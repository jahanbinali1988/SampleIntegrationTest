namespace SampleIntegrationTest.Api.Extensions
{
    internal static class WebApplicationExtensions
    {
        public static WebApplication Configure(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            return app;
        }
    }
}
