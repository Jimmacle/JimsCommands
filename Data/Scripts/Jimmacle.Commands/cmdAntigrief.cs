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

    class cmdAntigrief : ChatCommand
    {
        public cmdAntigrief() : base("Antigrief", "ag", "Manage anti-grief settings", 
            "This command gives access to the anti-grief system.\n" +
            "Subcommands: protect, unprotect, log\n\n" +
            "/ag protect [ship name]: Protects the grid with the specified name.\n" +
            "/ag unprotect [ship name]: Removes protection from the grid with the specified name.\n" +
            "/ag log: Opens the attack log file.") { }

        public override string Invoke(List<string> parameters)
        {
            //protect, unprotect - set grid flag

            //log - view log

            return "Command failed";
        }
    }
}
