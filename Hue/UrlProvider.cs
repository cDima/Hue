using Hue.Properties;

namespace Hue
{
    public class UrlProvider
    {
        private string ip;

        public UrlProvider(string ip)
        {
            this.ip = ip;

            if (!this.ip.StartsWith("http://"))
                this.ip = "http://" + this.ip;

            this.ip = this.ip.TrimEnd('/');
            this.ip = this.ip.Replace("/description.xml","");
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
