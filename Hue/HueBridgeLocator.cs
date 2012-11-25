using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hue
{
    public static class HueBridgeLocator
    {
        public static async Task<HueBridge> LocateBridge(List<string> IPs)
        {
            // http://www.nerdblog.com/2012/10/a-day-with-philips-hue.html - description.xml retrieval

            // This shoudl be a UDP 


            //IPs.AsParallel().ForAll(async ip =>
            foreach (var ip in IPs)
            {
                try
                {
                    // Easy protocol discovery
                    var url = "http://" + ip + "/description.xml";
                    var http = new HttpClient {Timeout = TimeSpan.FromMilliseconds(2000)};

                    var res = await http.GetStringAsync(url);
                    if (!string.IsNullOrWhiteSpace(res))
                    {
                        // we can also do this:
                        //var root = (root) new XmlSerializer(typeof (root)).Deserialize(stream);
                        //var name = root.device.friendlyName;

                        var bridgeName = XElement.Parse(res)
                                                    .Descendants(XName.Get("device", @"urn:schemas-upnp-org:device-1-0"))
                                                    .Descendants(XName.Get("friendlyName", "urn:schemas-upnp-org:device-1-0"))
                                                    .First()
                                                    .Value;

                        if (!string.IsNullOrWhiteSpace(bridgeName))
                            return new HueBridge(ip); // support only first bridge
                    }
                }
                catch (Exception ex)
                {
                    //catch all web exceptions - no router in sight
                }
            }

            return null;
        }
    }
}
