namespace Hue.Contracts
{
    public class HueLight
    {
        public HueLightState state { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string modelid { get; set; }
        public long swversion { get; set; }

        public static HueLight Parse(dynamic d)
        {
            var instance = new HueLight();
            instance.state = HueLightState.Parse(d["state"]);
            instance.type = d["type"];
            instance.name = d["name"];
            instance.modelid = d["modelid"];
            instance.swversion = long.Parse(d["swversion"]);
            return instance;
        }
    }

    public class HueLightState
    {
        public bool on { get; set; }
        public int bri { get; set; }
        public int hue { get; set; }
        public int sat { get; set; }
        public decimal[] xy { get; set; }
        public int ct { get; set; }
        public string alert { get; set; }
        public string effect { get; set; }
        public string colormode { get; set; }
        public bool reachable { get; set; }

        public static HueLightState Parse(dynamic d)
        {
            var instance = new HueLightState();

            instance.on = d["on"];
            instance.bri = d["bri"];
            instance.hue = d["hue"];
            instance.sat = d["sat"];
            instance.xy = new decimal[] { d["xy"][0], d["xy"][1] };
            instance.alert = d["alert"];
            instance.effect = d["effect"];
            instance.colormode = d["colormode"];
            instance.reachable = d["reachable"];
            return instance;
        }

        public string AsJsonCommand()
        {
            return JsonDiff(new HueLightState());
        }

        public string JsonDiff(HueLightState update)
        {
            var result = "{";

            if (bri != update.bri) result += "\"bri\": " + update.bri + ", ";
            if (on != update.on) result += "\"on\": " + update.on.ToString().ToLowerInvariant() + ", ";
            //if (hue != update.hue) result += "\"hue\": " + update.hue + ", ";
            //if (sat != update.sat) result += "\"sat\": " + update.sat + ", ";
            //if (alert != update.alert) result += "\"alert\": " + update.alert + ", ";
            //instance.xy = new decimal[] { d["xy"][0], d["xy"][1] };
            //instance.alert = d["alert"];
            //instance.effect = d["effect"];
            //instance.colormode = d["colormode"];
            //instance.reachable = d["reachable"];

            result = result.TrimEnd(',', ' ');
            result += "}";

            if (result == "{}")
                return "";
            return result;
        }

        internal static string JsonCommand(HueLightState hueLightState1, HueLightState hueLightState2)
        {
            return hueLightState1.JsonDiff(hueLightState2);
        }
    }
}
/*
{
  "lights": {
    "1": {
      "state": {
        "on": true,
        "bri": 219,
        "hue": 33863,
        "sat": 49,
        "xy": [
          0.368,
          0.3686
        ],
        "ct": 231,
        "alert": "none",
        "effect": "none",
        "colormode": "ct",
        "reachable": true
      },
      "type": "Extended color light",
      "name": "Hue Lamp 1",
      "modelid": "LCT001",
      "swversion": "65003148",
      "pointsymbol": {
        "1": "none",
        "2": "none",
        "3": "none",
        "4": "none",
        "5": "none",
        "6": "none",
        "7": "none",
        "8": "none"
      }
    },
    "2": {
      "state": {
        "on": true,
        "bri": 219,
        "hue": 33863,
        "sat": 49,
        "xy": [
          0.368,
          0.3686
        ],
        "ct": 231,
        "alert": "none",
        "effect": "none",
        "colormode": "ct",
        "reachable": true
      },
      "type": "Extended color light",
      "name": "Hue Lamp 2",
      "modelid": "LCT001",
      "swversion": "65003148",
      "pointsymbol": {
        "1": "none",
        "2": "none",
        "3": "none",
        "4": "none",
        "5": "none",
        "6": "none",
        "7": "none",
        "8": "none"
      }
    },
    "3": {
      "state": {
        "on": true,
        "bri": 219,
        "hue": 33863,
        "sat": 49,
        "xy": [
          0.368,
          0.3686
        ],
        "ct": 231,
        "alert": "none",
        "effect": "none",
        "colormode": "ct",
        "reachable": true
      },
      "type": "Extended color light",
      "name": "Hue Lamp 3",
      "modelid": "LCT001",
      "swversion": "65003148",
      "pointsymbol": {
        "1": "none",
        "2": "none",
        "3": "none",
        "4": "none",
        "5": "none",
        "6": "none",
        "7": "none",
        "8": "none"
      }
    }
  },
  "groups": {
    
  },
  "config": {
    "name": "Philips hue",
    "mac": "00:17:88:09:62:40",
    "dhcp": true,
    "ipaddress": "192.168.0.113",
    "netmask": "255.255.255.0",
    "gateway": "192.168.0.1",
    "proxyaddress": "",
    "proxyport": 0,
    "UTC": "2012-11-15T03:08:08",
    "whitelist": {
      "c20aca42279b2898bb1ce2a470da6d64": {
        "last use date": "2012-11-14T23:41:41",
        "create date": "2012-11-07T03:00:06",
        "name": "Dmitri Sadakov\u2019s iPhone"
      },
      "3b268b59109f63d7319c8f9d2a9d2edb": {
        "last use date": "2012-11-07T04:31:07",
        "create date": "2012-11-07T04:28:27",
        "name": "soapui"
      },
      "2cb1ac173bc8aa7f2cae5a073a11fa8f": {
        "last use date": "2012-11-12T02:40:02",
        "create date": "2012-11-07T04:28:44",
        "name": "soapui"
      },
      "26edc9a619306aa4b473ff22165751f": {
        "last use date": "2012-11-07T03:00:06",
        "create date": "2012-11-07T04:28:45",
        "name": "soapui"
      },
      "343855a103b881726d398c68ac6333": {
        "last use date": "2012-11-07T21:56:21",
        "create date": "2012-11-07T19:22:04",
        "name": "python_hue"
      },
      "b7a7e52143446771752ae6e1c69b0a3": {
        "last use date": "2012-11-07T21:56:21",
        "create date": "2012-11-13T04:31:39",
        "name": "WinHueApp"
      },
      "1ec60546129895441850019217b1753f": {
        "last use date": "2012-11-07T21:56:21",
        "create date": "2012-11-15T01:35:34",
        "name": "winhueapp"
      },
      "3fa667052b1747071bc90d137472433": {
        "last use date": "2012-11-07T21:56:21",
        "create date": "2012-11-15T02:20:50",
        "name": "winhueapp"
      },
      "28fd5ecc3add810fa0aaaa41e1db8a7": {
        "last use date": "2012-11-07T21:56:21",
        "create date": "2012-11-15T02:23:55",
        "name": "winhueapp"
      },
      "2c68b67e2d21c1c73e826292701a5eb": {
        "last use date": "2012-11-07T21:56:21",
        "create date": "2012-11-15T02:25:20",
        "name": "winhueapp"
      },
      "15706f6e1d8b9167d32b2822fe99f8b": {
        "last use date": "2012-11-15T02:31:25",
        "create date": "2012-11-15T02:30:31",
        "name": "winhueapp"
      },
      "1db73d762d1d8ea73c14bbda7fac1bb": {
        "last use date": "2012-11-07T21:56:21",
        "create date": "2012-11-15T03:00:44",
        "name": "winhueapp"
      },
      "f86f8213eacc771e26889e19d01083": {
        "last use date": "2012-11-15T03:08:08",
        "create date": "2012-11-15T03:07:53",
        "name": "winhueapp"
      }
    },
    "swversion": "01003542",
    "swupdate": {
      "updatestate": 0,
      "url": "",
      "text": "",
      "notify": false
    },
    "linkbutton": true,
    "portalservices": true
  },
  "schedules": {
    
  }
}
*/