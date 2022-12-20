using SampleIntegrationTest.Api.Extensions;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Configure();
try
{
    app.Run();
}
catch (Exception ex)
{
    Debugger.Break();
}

public partial class Program { }
