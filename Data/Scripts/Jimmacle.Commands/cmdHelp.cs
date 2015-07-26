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

    class cmdHelp : ChatCommand
    {
        public cmdHelp() : base("Help", "help", "Displays help about the commands") { }

        public override string Invoke(List<string> parameters)
        {
            string helpString = "Enter the base command of a feature to view more help about the command.\n\n";
            foreach (var cmd in Logic.Commands)
            {
                helpString += string.Format("{0} (/{1}): {2}\n\n", cmd.Name, cmd.Command, cmd.ShortHelp);
            }
            MyAPIGateway.Utilities.TryShowWindow("Help", helpString);
            return null;
        }
    }
}
