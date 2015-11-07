using System;

namespace HueTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var bridge = Hue.HueBridgeLocator.Locate();
            Console.WriteLine((bridge == null) ? "Bridge was not found." : "Bridge found at " + bridge.IP);

            if (bridge != null)
            {
                bridge.PushButtonOnBridge += bridge_PushButtonOnBridge;

                bridge.FlashLights();
            }

            Console.ReadKey();
        }

        static void bridge_PushButtonOnBridge(object sender, EventArgs e)
        {
            Console.WriteLine("Please press the button on the bridge to register the application in the next minute.");
        }
    }
}
