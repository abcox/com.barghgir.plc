using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.web.Services
{
    public class DataProtectionService
    {
        private readonly IDataProtectionProvider dataProtectionProvider;

        public DataProtectionService(IDataProtectionProvider dataProtectionProvider)
        {
            this.dataProtectionProvider = DataProtectionProvider
                .Create(AppDomain.CurrentDomain.FriendlyName)
                .CreateProtector(""); // .Replace(".", "-"));
        }

        public string GetProtectedData(string data)
        {
            var protector = dataProtectionProvider.CreateProtector("");
            var protectedData = protector.Protect(data);
            return protectedData;
        }
    }
}
