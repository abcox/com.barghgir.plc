using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.web.Helpers
{
    public class HttpsClientHandlerService
    {
        public HttpMessageHandler GetPlatformMessageHandler()
        {
#if ANDROID
            var handler = new Xamarin.Android.Net.AndroidMessageHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert != null && cert.Issuer.Equals("CN=Keyoti Conveyor Root Certificate Authority 2 - For development testing only!")) // string was "CN=localhost"
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
#elif IOS
    var handler = new NSUrlSessionHandler
    {
        TrustOverrideForUrl = IsHttpsLocalhost
    };
    return handler;
#else
            throw new PlatformNotSupportedException("Only Android and iOS supported.");
#endif
        }

#if IOS
public bool IsHttpsLocalhost(NSUrlSessionHandler sender, string url, Security.SecTrust trust)
{
    if (url.StartsWith("https://localhost"))
        return true;
    return false;
}
#endif
    }
}

