using com.barghgir.plc.api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<ApiOptions>().BindConfiguration(""); // reads the whole appset

var services = builder.Services;

// Add services to the container.

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

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
}); ;
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
