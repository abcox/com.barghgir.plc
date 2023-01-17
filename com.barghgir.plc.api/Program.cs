using com.barghgir.plc.api.Data;
using com.barghgir.plc.common.Configuration;
using com.barghgir.plc.data.Context;
using com.barghgir.plc.infra;
using com.barghgir.plc.infra.Security.Token;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Extensions.Hosting;
using Serilog.Settings.Configuration;
using System.Text.Json.Serialization;

const string DEFAULT_ENVIRONMENT_NAME = "development";

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? DEFAULT_ENVIRONMENT_NAME;
var appsettingsFileName = $"appsettings.{env}.json";

if (!File.Exists(appsettingsFileName))
    throw new ApplicationException($"file {appsettingsFileName} not found");

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile(appsettingsFileName, false, true);
if (env.Equals(DEFAULT_ENVIRONMENT_NAME))
    builder.AddSerilog();

var services = builder.Services;

services.AddOptions<ApiOptions>().BindConfiguration(string.Empty); // reads whole appsettings file

// Add services to the container.
services.AddDbContext<CcaDevContext>(options => {
    var connectionString = builder.Configuration.GetConnectionString(nameof(AppDbContext));
    options.UseSqlServer(connectionString);
});
services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddJwt();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    var thisNamespace = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{thisNamespace} (dark)");
    c.InjectStylesheet("/swagger-ui/swagger-ui-dark.css"); // https://dev.to/amoenus/turn-swagger-theme-to-the-dark-mode-4l5f
});
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
