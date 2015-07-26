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

    public class Antigrief
    {
        public static void DamageHandler(Object obj, ref MyDamageInformation info)
        {
            try
            {
                IMySlimBlock targetBlock = obj as IMySlimBlock;
                IMyCubeGrid targetGrid = targetBlock.CubeGrid;

                if (Storage.Data.Grids.Grids.Find(g => g.Id == targetGrid.EntityId) != null)
                {
                    info.Amount = 0f;
                }

                return;
            }
            catch { }
        }
    }
}
