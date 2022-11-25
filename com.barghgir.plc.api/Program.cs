using com.barghgir.plc.api.Data;
using com.barghgir.plc.common.Configuration;
using com.barghgir.plc.infra;
using com.barghgir.plc.infra.Security.Token;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Extensions.Hosting;
using Serilog.Settings.Configuration;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<ApiOptions>().BindConfiguration(""); // reads the whole appset

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (!string.IsNullOrEmpty(env))
{
    if (File.Exists($"appsettings.{env}.json"))
        builder.Configuration.AddJsonFile($"appsettings.{env}.json", false, true);
    if (env.ToLower().StartsWith("dev"))
        builder.AddSerilog();
}

var services = builder.Services;

// Add services to the container.
services.AddDbContext<AppDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString(nameof(AppDbContext))));
services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
services.AddControllers();
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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "com.barghgir.plc.api (dark)");
    c.InjectStylesheet("/swagger-ui/swagger-ui-dark.css"); // https://dev.to/amoenus/turn-swagger-theme-to-the-dark-mode-4l5f
});
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
