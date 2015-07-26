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
    using VRage.ModAPI;

    class cmdAntigrief : ChatCommand
    {
        public cmdAntigrief() : base("Antigrief", "ag", "Manage anti-grief settings", 
            "This command gives access to the anti-grief system.\n\n" +
            "/ag protect \"[ship name]\": Protects the grid with the specified name.\n" +
            "/ag unprotect \"[ship name]\": Removes protection from the grid with the specified name.\n" +
            "/ag list: Lists protected grids.") { }

        public override string Invoke(List<string> parameters)
        {
            if (parameters.Count == 1)
            {
                MyAPIGateway.Utilities.TryShowWindow(this.Name, this.LongHelp);
                return null;
            }

            if (parameters.Count == 3)
            {
                IMyCubeGrid grid = Utilities.GetGrid(parameters[2]);

                if (grid == null)
                {
                    return "Grid not found";
                }

                switch (parameters[1])
                {
                    case "protect":
                        Storage.Data.Grids.Grids.Add(new Grid(grid.EntityId, true));
                        return null;

                    case "unprotect":
                        Storage.Data.Grids.Grids.RemoveAll(g => g.Id == grid.EntityId);
                        return null;
                }
            }

            if (parameters.Count == 2 && parameters[1] == "list")
            {
                string display = "";
                foreach (var g in Storage.Data.Grids.Grids)
                {
                    display += Utilities.GetGrid(g.Id).DisplayName + "\n";
                }
                MyAPIGateway.Utilities.TryShowWindow(this.Name, display);
                return null;
            }


            //protect, unprotect - set grid flag

            //log - view log

            return "Command failed";
        }
    }
}
