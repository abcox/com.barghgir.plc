using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.web.Helpers
{
    public static class HttpHelper
    {
        public static HttpMessageHandler GetPlatformMessageHandler()
        {
            var issuers = new List<string>();
            issuers.Add("CN=Microsoft Azure TLS Issuing CA 01, O=Microsoft Corporation, C=US");
#if DEBUG
            issuers.Add("CN=localhost");
            issuers.Add("CN=Keyoti Conveyor Root Certificate Authority 2 - For development testing only!");
#endif
#if ANDROID
            var handler = new Xamarin.Android.Net.AndroidMessageHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert != null && issuers.Contains(cert.Issuer))
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
        public static bool IsHttpsLocalhost(NSUrlSessionHandler sender, string url, Security.SecTrust trust)
        {
            if (url.StartsWith("https://localhost"))
                return true;
            return false;
        }
#endif

        public static HttpClient GetHttpClient()
        {
            HttpClient httpClient;
#if DEBUG
            //HttpsClientHandlerService handler = new HttpsClientHandlerService();
            //HttpClient httpClient = new HttpClient(handler.GetPlatformMessageHandler());

            //HttpClient httpClient = new DevHttpsConnectionHelper(7132).HttpClient;
            httpClient = new HttpClient(GetPlatformMessageHandler());
#else
            httpClient = new HttpClient();
#endif
            return httpClient;
        }
    }
}

