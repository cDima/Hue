#  Philips Hue lights .Net C# wrapper library 
## A wrapper for Philips Hue lights rest api for C# 4.5

This is a library for the philips hue lights written in C#.

Eample code:

       var hue = new HueBridge("192.168.1.113"); 
       hue.FlashLights();


## SSDP Locator

Automatically find the Philips Hue bridge in a network like this:

            var bridge = Hue.HueBridgeLocator.Locate();
			if (bridge != null)
				bridge.FlashLights();
            
## Automatic registration

To control the lights, the client needs to authenticate with the bridge. The client fires the PushButtonOnBridge event if it needs registration:

        bridge.PushButtonOnBridge += bridge_PushButtonOnBridge;
		...
	
        static void bridge_PushButtonOnBridge(object sender, EventArgs e)
        {
            Console.WriteLine("Please press the button on the bridge to register the application in the next minute.");
        }

