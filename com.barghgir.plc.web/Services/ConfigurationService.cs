using com.barghgir.plc.common.Configuration;
using com.barghgir.plc.common.Helpers;
using com.barghgir.plc.data.Models;
using com.barghgir.plc.web.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static com.barghgir.plc.data.Models.Configuration;
using Environment = com.barghgir.plc.data.Models.Configuration.Environment;

namespace com.barghgir.plc.web.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public readonly string EnvironmentConfigurationJsonFilename = "configuration.json";
        private readonly ILogger logger;

        private Environment environment;
        private ApiOptions options;

        public ConfigurationService(ILogger<ConfigurationService> logger)
        {
            this.logger = logger;
            //Environment = GetEnvironment();
            //Options = GetAppConfigAsync();
        }

        public string BaseAddress { get { return Environment?.Options?.BaseServiceEndpoint; } }

        public Environment Environment
        {
            get
            {
                if (environment == null)
                {
                    environment = GetEnvironment().Result;
                }
                return environment;
            }
        }

        public ApiOptions Options
        {
            get
            {
                if (options == null)
                {
                    options = GetAppConfigAsync();
                }
                return options;
            }
        }

        public async Task<Environment> GetEnvironment()
        {
            Environment environment;
            try
            {
                var configuration = await FileHelpers
                    .GetDeserializedContent<Configuration>(EnvironmentConfigurationJsonFilename);
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

        // TODO: figure out how to send notifications to present user with feedback like "successful" or "error" *******

        public ApiOptions GetAppConfigAsync()
        {
            ApiOptions options = null;
            try
            {
                var url = $"{BaseAddress}/configuration/app";
                var response = HttpHelper.GetHttpClient().GetAsync(url).Result;

                if (!response.IsSuccessStatusCode)
                    throw new ApplicationException("Failed to get configuration");

                options = response.Content.ReadFromJsonAsync<ApiOptions>().Result;
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to get config: {exceptionMessage}", ex.Message);
            }
            return options;
        }
    }
}
