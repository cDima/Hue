using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hue
{
    /// <summary>
    /// Locates the Philips Hue lights bridge using SSDP 
    /// </summary>
    public static class HueBridgeLocator
    {
        public static HueBridge Locate()
        {
            //https://www.meethue.com/api/nupnp
            //return LocateAsync().Result;
            return new HueBridge("192.168.0.102");
        }

        public static async Task<HueBridge> LocateAsync()
        {
            if (UPnP.NAT.Discover())
            {
                var endpoints = UPnP.NAT.DiscoveredEndpoints
                    .Where(s => s.EndsWith("/description.xml")).ToList();
                foreach (var endpoint in endpoints)
                {
                    if (await IsHue(endpoint))
                    {
                        var ip = endpoint.Replace("http://", "").Replace("/description.xml", "");
                        return new HueBridge(ip);
                    }
                }
                return null;
            }
            return null;
        }

        // http://www.nerdblog.com/2012/10/a-day-with-philips-hue.html - description.xml retrieval
        private static async Task<bool> IsHue(string discoveryUrl)
        {
            var http = new HttpClient {Timeout = TimeSpan.FromMilliseconds(2000)};
            try {
                var res = await http.GetStringAsync(discoveryUrl);
                if (!string.IsNullOrWhiteSpace(res))
                {
                    if (res.Contains("Philips hue bridge"))
                        return true;
                }
            } catch
            {
                return false;
            }
            return false;
        }
    }
}
