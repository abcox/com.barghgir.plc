using com.barghgir.plc.data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static com.barghgir.plc.data.Models.Configuration;
using Environment = com.barghgir.plc.data.Models.Configuration.Environment;

namespace com.barghgir.plc.web.Services
{
    public static class ConfigurationService
    {
        public static readonly string EnvironmentConfigurationJsonFilename = "configuration.json";

        public static async Task<Environment> GetEnvironment()
        {
            Environment? environment;
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(EnvironmentConfigurationJsonFilename);
                using var reader = new StreamReader(stream);
                var contents = await reader.ReadToEndAsync();
                var configuration = JsonSerializer.Deserialize<Configuration>(contents);
                environment = configuration.Environments?
                    .FirstOrDefault(x => x.Name == configuration.SelectedEnvironmentName);

                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    // configure Android specific things

                    // "https://com-barghgir-plc-api.azurewebsites.net" /* "https://192.168.2.53:45455" /* "http://localhost:5260" "https://10.0.2.2:45455" "https://192.168.2.53:45455" "https://10.0.2.2:5001" */ : "https://localhost:7132";
                }

                if (environment == null)
                    throw new ApplicationException("Configuration required");
            }
            catch (Exception ex)
            {
                // log error!
                throw;
            }
            return environment;
        }

    }
}
