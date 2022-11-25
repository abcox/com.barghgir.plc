using com.barghgir.plc.common.Configuration;
using com.barghgir.plc.api.Helpers;
using com.barghgir.plc.data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace com.barghgir.plc.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly ILogger<ConfigurationController> logger;
        private readonly ApiOptions options;

        public ConfigurationController(ILogger<ConfigurationController> logger, IOptions<ApiOptions> options)
        {
            this.logger = logger;

            if (options?.Value != null) { this.logger.LogWarning("Api config failure"); }
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