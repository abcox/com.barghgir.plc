using com.barghgir.plc.common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Environment = com.barghgir.plc.data.Models.Configuration.Environment;

namespace com.barghgir.plc.web.Services
{
    public interface IConfigurationService
    {
        Task<Environment> GetEnvironment();
        ApiOptions GetAppConfigAsync();
    }
}
