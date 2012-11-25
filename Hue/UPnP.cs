using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace UPnP
{
    public class NAT
    {
        static readonly TimeSpan timeout = TimeSpan.FromSeconds(3);

        public static List<string> DiscoveredEndpoints = new List<string>();

        public static bool Discover()
        {
            var s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            
            string req = "M-SEARCH * HTTP/1.1\r\n" +
                         "HOST: 239.255.255.250:1900\r\n" +
                         "ST:upnp:rootdevice\r\n" +
                         "MAN:\"ssdp:discover\"\r\n" +
                         "MX:3\r\n\r\n";

            var data = Encoding.ASCII.GetBytes(req);
            var ipe = new IPEndPoint(IPAddress.Broadcast, 1900);
            var buffer = new byte[0x1000];

            var start = DateTime.Now;

            DiscoveredEndpoints = new List<string>();
            do
            {
                s.SendTo(data, ipe);
                
                int length = 0;
                do
                {
                    if (s.Available == 0)  
                        break;// nothing more to read

                    length = s.Receive(buffer);
                    var resp = Encoding.ASCII.GetString(buffer, 0, length).ToLower();

                    var location  = resp.Substring(resp.ToLower().IndexOf("location:", System.StringComparison.Ordinal) + 9);
                    resp = location.Substring(0, location.IndexOf("\r", System.StringComparison.Ordinal)).Trim();
                     
                    if (!DiscoveredEndpoints.Contains(resp))
                        DiscoveredEndpoints.Add(resp);

                }
                while (length > 0);
            }
            while (DateTime.Now.Subtract(start) < timeout); // try for thee seconds

            if (DiscoveredEndpoints.Count != 0)
                return true;

            return false;
        }
    }
}
