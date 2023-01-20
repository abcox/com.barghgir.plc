using Azure.Identity;
using com.barghgir.plc.api.Data;
using com.barghgir.plc.common.Configuration;
using com.barghgir.plc.data.Context;
using com.barghgir.plc.infra;
using com.barghgir.plc.infra.Security.Token;
using com.barghgir.plc.api.Helpers;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Abstractions;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Events;
using Serilog.Extensions.Hosting;
using Serilog.Settings.Configuration;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Azure.Extensions.AspNetCore.Configuration.Secrets;

const string AZURE_CLIENT_ID = "AZURE_CLIENT_ID";
const string AZURE_CLIENT_SECRET = "AZURE_CLIENT_SECRET";
const string AZURE_TENANT_ID = "AZURE_TENANT_ID";
const string ASPNETCORE_ENVIRONMENT_NAME = "ASPNETCORE_ENVIRONMENT";
const string DEFAULT_ENVIRONMENT_NAME = "Release";

var builder = WebApplication.CreateBuilder(args);
var isProduction = builder.Environment?.IsProduction() ?? true;

if (builder == null)
    throw new ArgumentNullException(nameof(builder));

var configuration = builder.Configuration;
if (configuration == null)
    throw new ArgumentNullException(nameof(configuration));

var env = Environment.GetEnvironmentVariable(ASPNETCORE_ENVIRONMENT_NAME) ?? DEFAULT_ENVIRONMENT_NAME;
if (env == null)
    throw new ArgumentNullException(nameof(configuration));

var appsettingsFileName = env.Equals(DEFAULT_ENVIRONMENT_NAME) ?
    "appsettings.json" : $"appsettings.{env}.json";

if (!File.Exists(appsettingsFileName))
    throw new ApplicationException($"File not found: {appsettingsFileName}");

configuration.AddJsonFile(appsettingsFileName, false, true);

//if (env.Equals(DEFAULT_ENVIRONMENT_NAME))
builder.AddSerilog();

var services = builder.Services;

services.AddOptions<ApiOptions>().BindConfiguration(string.Empty); // reads whole appsettings file

var thisNamespace = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
var options = configuration.Get<ApiOptions>();

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
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = thisNamespace, Version = "v1" });
});
services.AddJwt();

var app = builder.Build();

ILogger<Program> logger = app.Services.GetRequiredService<ILogger<Program>>();
//logger.LogInformation("Starting...");
//logger.LogInformation($"Environment: {env}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // nothing here yet!
}
else
{
    // nothing here yet!
}

// Azure Active Directory (AAD) / App registrations / com.barghgir.plc(Secret Permissions: Get, List, etc.): https://portal.azure.com/?feature.msaljs=false#view/Microsoft_AAD_RegisteredApps/ApplicationMenuBlade/~/Overview/appId/56c4326a-6c85-45e7-932a-050350068559/isMSAApp~/false
// Key vault(cca-cc-rg-01-kv) / Access policies / com.barghgir.plc: https://portal.azure.com/?feature.msaljs=false#@Vorba.onmicrosoft.com/resource/subscriptions/236217f7-0ad4-4dd6-8553-dc4b574fd2c5/resourceGroups/cca-cc-rg-01/providers/Microsoft.KeyVault/vaults/cca-cc-rg-01-kv/access_policies
Environment.SetEnvironmentVariable(AZURE_CLIENT_ID, options?.Azure?.Environment?.ClientId);
//Environment.SetEnvironmentVariable(AZURE_CLIENT_SECRET, options?.Azure?.Environment?.ClientSecret); // TODO: this value needs to be setup in the appsvc resource prior to publishing --> make release pipeline tasks that setup the resources and configure them
Environment.SetEnvironmentVariable(AZURE_TENANT_ID, options?.Azure?.Environment?.TenantId);

//var keyVauleUrl = $"https://{keyVaultName}.vault.azure.net/";
string keyVaultUrl = options?.Azure?.KeyVault?.Url ?? string.Empty;
if (string.IsNullOrEmpty(keyVaultUrl))
{
    logger.LogCritical($"Appsetting not found: {nameof(keyVaultUrl)}");
    throw new ApplicationException($"Appsetting not found: {nameof(keyVaultUrl)}");
}
configuration.AddAzureKeyVault(
    new Uri(keyVaultUrl),
    new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = options?.Azure?.Environment?.ClientId }),
    new AzureKeyVaultConfigurationOptions { ReloadInterval = TimeSpan.FromMinutes(1), Manager = new CustomKeyVaultSecretManager(options?.Azure?.KeyVault?.Prefix ?? string.Empty) }
);

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{thisNamespace} ({env})");
    c.InjectStylesheet("/swagger-ui/swagger-ui-dark.css"); // https://dev.to/amoenus/turn-swagger-theme-to-the-dark-mode-4l5f
});
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
