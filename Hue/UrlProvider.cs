using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hue.Properties;

namespace Hue
{
    public class UrlProvider
    {
        private string ip;

        public UrlProvider(string ip)
        {
            this.ip = ip;

            if (!ip.StartsWith("http://")) this.ip = "http://" + this.ip;

            this.ip = this.ip.TrimEnd('/');
        }

        internal string GetStatusUrl()
        {
            return ip + "/api/" + Settings.Default.BridgeApiKey;
        }

        internal string GetRegisterUrl()
        {
            return ip + "/api";
        }

        internal string GetLampUrl(string lightKey)
        {
            return ip + "/api/" + Settings.Default.BridgeApiKey + "/lights/" + lightKey + "/state";
        }
    }
}
