using Hue.Contracts;
using Hue.JSON;
using Hue.Properties;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Hue
{
    public class HueBridge
    {
        public UrlProvider Urls;
        public ConcurrentDictionary<string, HueLight> Lights { get; set; }

        public event EventHandler PushButtonOnBridge;  

        private readonly string appname = "winhueapp";
        private readonly string ip;
        private Timer timer;
        private bool IsAuthenticated = false;
        
        public HueBridge(string ip)
        {
            Urls = new UrlProvider(ip);
            this.ip = ip;
            // not needed - clock for every 1 sec update status. 
            //timer = new Timer(StatusCheckEvent, null, 0, 1000);
        }

        public async Task<bool> InitializeRouter()
        {
            if (!string.IsNullOrEmpty(Settings.Default.BridgeApiKey))
            {
                TryUpdateLights();
                if (IsAuthenticated) return true;
            }

            return await Register();
        }

        private void StatusCheckEvent(object state)
        {
            // read state of lamps
            if (IsAuthenticated) 
                TryUpdateLights();
        }

        private void TryUpdateLights()
        {
            var url = Urls.GetStatusUrl();
            var statusResponse = new HttpClient().GetStringAsync(url).Result;

            // error response:
            //[{"error":{"type":1,"address":"/lights","description":"unauthorized user"}}]
            if (!statusResponse.Contains("unauthorized user"))
            {
                ParseLights(statusResponse);
                //{"lights":{"1":{"state": {"on":true,"bri":219,"hue":33863,"sat":49,"xy":[0.3680,0.3686],"ct":231,"alert":"none","effect":"none","colormode":"ct","reachable":true}, "type": "Extended color light", "name": "Hue Lamp 1", "modelid": "LCT001", "swversion": "65003148", "pointsymbol": { "1":"none", "2":"none", "3":"none", "4":"none", "5":"none", "6":"none", "7":"none", "8":"none" }},"2":{"state": {"on":true,"bri":219,"hue":33863,"sat":49,"xy":[0.3680,0.3686],"ct":231,"alert":"none","effect":"none","colormode":"ct","reachable":true}, "type": "Extended color light", "name": "Hue Lamp 2", "modelid": "LCT001", "swversion": "65003148", "pointsymbol": { "1":"none", "2":"none", "3":"none", "4":"none", "5":"none", "6":"none", "7":"none", "8":"none" }},"3":{"state": {"on":true,"bri":219,"hue":33863,"sat":49,"xy":[0.3680,0.3686],"ct":231,"alert":"none","effect":"none","colormode":"ct","reachable":true}, "type": "Extended color light", "name": "Hue Lamp 3", "modelid": "LCT001", "swversion": "65003148", "pointsymbol": { "1":"none", "2":"none", "3":"none", "4":"none", "5":"none", "6":"none", "7":"none", "8":"none" }}},"groups":{},"config":{"name": "Philips hue","mac": "00:17:88:09:62:40","dhcp": true,"ipaddress": "192.168.0.113","netmask": "255.255.255.0","gateway": "192.168.0.1","proxyaddress": "","proxyport": 0,"UTC": "2012-11-15T03:08:08","whitelist":{"c20aca42279b2898bb1ce2a470da6d64":{"last use date": "2012-11-14T23:41:41","create date": "2012-11-07T03:00:06","name": "Dmitri Sadakov’s iPhone"},"3b268b59109f63d7319c8f9d2a9d2edb":{"last use date": "2012-11-07T04:31:07","create date": "2012-11-07T04:28:27","name": "soapui"},"2cb1ac173bc8aa7f2cae5a073a11fa8f":{"last use date": "2012-11-12T02:40:02","create date": "2012-11-07T04:28:44","name": "soapui"},"26edc9a619306aa4b473ff22165751f":{"last use date": "2012-11-07T03:00:06","create date": "2012-11-07T04:28:45","name": "soapui"},"343855a103b881726d398c68ac6333":{"last use date": "2012-11-07T21:56:21","create date": "2012-11-07T19:22:04","name": "python_hue"},"b7a7e52143446771752ae6e1c69b0a3":{"last use date": "2012-11-07T21:56:21","create date": "2012-11-13T04:31:39","name": "WinHueApp"},"1ec60546129895441850019217b1753f":{"last use date": "2012-11-07T21:56:21","create date": "2012-11-15T01:35:34","name": "winhueapp"},"3fa667052b1747071bc90d137472433":{"last use date": "2012-11-07T21:56:21","create date": "2012-11-15T02:20:50","name": "winhueapp"},"28fd5ecc3add810fa0aaaa41e1db8a7":{"last use date": "2012-11-07T21:56:21","create date": "2012-11-15T02:23:55","name": "winhueapp"},"2c68b67e2d21c1c73e826292701a5eb":{"last use date": "2012-11-07T21:56:21","create date": "2012-11-15T02:25:20","name": "winhueapp"},"15706f6e1d8b9167d32b2822fe99f8b":{"last use date": "2012-11-15T02:31:25","create date": "2012-11-15T02:30:31","name": "winhueapp"},"1db73d762d1d8ea73c14bbda7fac1bb":{"last use date": "2012-11-07T21:56:21","create date": "2012-11-15T03:00:44","name": "winhueapp"},"f86f8213eacc771e26889e19d01083":{"last use date": "2012-11-15T03:08:08","create date": "2012-11-15T03:07:53","name": "winhueapp"}},"swversion": "01003542","swupdate":{"updatestate":0,"url":"","text":"","notify": false},"linkbutton": true,"portalservices": true},"schedules":{}}
                // /lights: {"1":{"name": "Hue Lamp 1"},"2":{"name": "Hue Lamp 2"},"3":{"name": "Hue Lamp 3"}}
                IsAuthenticated = true;
            }
        }

        private void ParseLights(string json)
        {
            //{"lights":{"1":{"state": {"on":true,"bri":219,"hue":33863,"sat":49,"xy":[0.3680,0.3686],"ct":231,"alert":"none","effect":"none","colormode":"ct","reachable":true}, "type": "Extended color light", "name": "Hue Lamp 1", "modelid": "LCT001", "swversion": "65003148", "pointsymbol": { "1":"none", "2":"none", "3":"none", "4":"none", "5":"none", "6":"none", "7":"none", "8":"none" }},"2":{"state": {"on":true,"bri":219,"hue":33863,"sat":49,"xy":[0.3680,0.3686],"ct":231,"alert":"none","effect":"none","colormode":"ct","reachable":true}, "type": "Extended color light", "name": "Hue Lamp 2", "modelid": "LCT001", "swversion": "65003148", "pointsymbol": { "1":"none", "2":"none", "3":"none", "4":"none", "5":"none", "6":"none", "7":"none", "8":"none" }},"3":{"state": {"on":true,"bri":219,"hue":33863,"sat":49,"xy":[0.3680,0.3686],"ct":231,"alert":"none","effect":"none","colormode":"ct","reachable":true}, "type": "Extended color light", "name": "Hue Lamp 3", "modelid": "LCT001", "swversion": "65003148", "pointsymbol": { "1":"none", "2":"none", "3":"none", "4":"none", "5":"none", "6":"none", "7":"none", "8":"none" }}},"groups":{},"config":{"name": "Philips hue","mac": "00:17:88:09:62:40","dhcp": true,"ipaddress": "192.168.0.113","netmask": "255.255.255.0","gateway": "192.168.0.1","proxyaddress": "","proxyport": 0,"UTC": "2012-11-15T03:08:08","whitelist":{"c20aca42279b2898bb1ce2a470da6d64":{"last use date": "2012-11-14T23:41:41","create date": "2012-11-07T03:00:06","name": "Dmitri Sadakov’s iPhone"},"3b268b59109f63d7319c8f9d2a9d2edb":{"last use date": "2012-11-07T04:31:07","create date": "2012-11-07T04:28:27","name": "soapui"},"2cb1ac173bc8aa7f2cae5a073a11fa8f":{"last use date": "2012-11-12T02:40:02","create date": "2012-11-07T04:28:44","name": "soapui"},"26edc9a619306aa4b473ff22165751f":{"last use date": "2012-11-07T03:00:06","create date": "2012-11-07T04:28:45","name": "soapui"},"343855a103b881726d398c68ac6333":{"last use date": "2012-11-07T21:56:21","create date": "2012-11-07T19:22:04","name": "python_hue"},"b7a7e52143446771752ae6e1c69b0a3":{"last use date": "2012-11-07T21:56:21","create date": "2012-11-13T04:31:39","name": "WinHueApp"},"1ec60546129895441850019217b1753f":{"last use date": "2012-11-07T21:56:21","create date": "2012-11-15T01:35:34","name": "winhueapp"},"3fa667052b1747071bc90d137472433":{"last use date": "2012-11-07T21:56:21","create date": "2012-11-15T02:20:50","name": "winhueapp"},"28fd5ecc3add810fa0aaaa41e1db8a7":{"last use date": "2012-11-07T21:56:21","create date": "2012-11-15T02:23:55","name": "winhueapp"},"2c68b67e2d21c1c73e826292701a5eb":{"last use date": "2012-11-07T21:56:21","create date": "2012-11-15T02:25:20","name": "winhueapp"},"15706f6e1d8b9167d32b2822fe99f8b":{"last use date": "2012-11-15T02:31:25","create date": "2012-11-15T02:30:31","name": "winhueapp"},"1db73d762d1d8ea73c14bbda7fac1bb":{"last use date": "2012-11-07T21:56:21","create date": "2012-11-15T03:00:44","name": "winhueapp"},"f86f8213eacc771e26889e19d01083":{"last use date": "2012-11-15T03:08:08","create date": "2012-11-15T03:07:53","name": "winhueapp"}},"swversion": "01003542","swupdate":{"updatestate":0,"url":"","text":"","notify": false},"linkbutton": true,"portalservices": true},"schedules":{}}

            var jss = new JavaScriptSerializer();
            var d = jss.Deserialize<dynamic>(json);
            var lights = d["lights"];
            
            Lights = new ConcurrentDictionary<string, HueLight>();
            foreach (var light in lights)
            {
                Lights.TryAdd(light.Key, HueLight.Parse(light.Value));
            }
        }

        private async Task<bool> Register()
        {
            var retryCount = 0;
            const int retryMax = 60;
            const int pauseMilliseconds = 1000;

            while (retryCount < retryMax) // wait a minute, check each second
            {
                var body = "{\"username\": \"" + appname + "\", \"devicetype\":\"" + appname + "\"}";
                var responseFromServer = await HttpRestHelper.Post(Urls.GetRegisterUrl(), body);

                if (responseFromServer.Contains("link button not pressed"))
                {
                    //responseFromServer = "[{\"error\":{\"type\":7,\"address\":\"/username\",\"description\":\"invalid value, winhueapp, for parameter, username\"}},{\"error\":{\"type\":101,\"address\":\"\",\"description\":\"link button not pressed\"}}]"
                    // link button not pressed, inform on first try only
                    if (retryCount == 0 && PushButtonOnBridge != null)
                        PushButtonOnBridge(this, null);

                    Thread.Sleep(pauseMilliseconds); // sleep for a second, then retry
                    retryCount++;
                }
                else
                {
                    dynamic obj = DynamicJsonConverter.Parse(responseFromServer);
                    // sample response: [{"error":{"type":7,"address":"/username","description":"invalid value, WinHueApp, for parameter, username"}},{"success":{"username":"b7a7e52143446771752ae6e1c69b0a3"}}]
                    
                    string key = obj[1].success.username;

                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        Settings.Default.BridgeApiKey = key;
                        Settings.Default.Save();
                    
                        IsAuthenticated = true;
                        return true;
                    }
                }
            }

            IsAuthenticated = false;
            return false;
        }

        private async void SetLightStatus(string lightKey, string json)
        {
            await HttpRestHelper.Put(Urls.GetLampUrl(lightKey), json);
        }

        public void AlertAllLights()
        {
            if (Lights != null && IsAuthenticated)
            {
                foreach (var light in Lights)
                     SetLightStatus(light.Key, "{\"alert\": \"select\" }");
            }
        }

        public void FlashLights()
        {
            if (Lights != null && IsAuthenticated)
            {
                foreach (var light in Lights)
                {
                    SetLightStatus(light.Key, "{\"bri\": 254, \"on\": true }");
                }
                Thread.Sleep(1000);
                foreach (var light in Lights)
                {
                    SetLightStatus(light.Key, "{\"bri\": 0, \"on\": true }");
                }
            }
        }

        public void TurnOffLights()
        {
            if (Lights != null && IsAuthenticated)
            {
                // push PUT request to /api/key/lights/1/state
                foreach (var light in Lights)
                {
                    SetLightStatus(light.Key, "{\"on\": false }");
                    //HueLightState.JsonCommand(new HueLightState{ on = true }, new HueLightState() { on = false }));
                }
            }
        }
    }
}