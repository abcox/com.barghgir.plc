using com.barghgir.plc.common.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace com.barghgir.plc.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly ApiOptions options;
        private readonly ILogger<ConfigurationController> logger;

        public ConfigurationController(
            IOptions<ApiOptions> options,
            ILogger<ConfigurationController> logger)
        {
            this.logger = logger;

            if (options?.Value == null)
            {
                this.logger.LogWarning("Api config failure");
            }
            this.options = options?.Value ?? new ApiOptions { };
        }

        [HttpGet]
        [Route("app", Name = "GetAppConfig")]
        public ApiOptions? GetAppConfig()
        {
            return options;
        }
    }
}