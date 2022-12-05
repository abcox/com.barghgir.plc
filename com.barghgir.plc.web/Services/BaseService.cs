using com.barghgir.plc.common.Configuration;
using com.barghgir.plc.data.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Environment = com.barghgir.plc.data.Models.Configuration.Environment;

namespace com.barghgir.plc.web.Services
{
    public class BaseService
    {
        private ApiOptions appConfig;
        private Environment environment;
        public string BaseAddress { get; }

        public readonly IConfigurationService configService;

        public BaseService(IConfigurationService configService)
        {
            this.configService = configService;
            BaseAddress = Environment.Options.BaseServiceEndpoint;
        }

        public Environment Environment
        {
            get
            {
                if (environment == null)
                    environment = configService.GetEnvironment().Result;

                return environment;
            }
        }

        public ApiOptions AppConfig
        {
            get
            {
                if (appConfig == null)
                    appConfig = configService.GetAppConfigAsync();

                return appConfig;
            }
        }
    }
}
