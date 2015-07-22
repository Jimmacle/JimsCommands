namespace Jimmacle.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Sandbox;
    using Sandbox.Common;
    using Sandbox.Definitions;
    using Sandbox.Game;
    using Sandbox.ModAPI;

    using VRage;
    using VRageMath;

    public class Network
    {
        public const ushort CLIENT_ID = 65534;
        public const ushort SERVER_ID = 65535;

        public static void MessageHandler(byte[] message)
        {
            Logger.WriteLine("log.txt", message.GetString());
        }
    }
}
