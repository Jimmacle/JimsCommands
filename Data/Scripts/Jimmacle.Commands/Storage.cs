namespace Jimmacle.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.IO;

    using Sandbox;
    using Sandbox.Common;
    using Sandbox.Definitions;
    using Sandbox.Game;
    using Sandbox.ModAPI;

    using VRage;
    using VRageMath;

    public class Storage
    {
        public static Data Data;

        public static void Load()
        {
            try
            {
                TextReader reader = MyAPIGateway.Utilities.ReadFileInGlobalStorage("JimsCommands_" + MyAPIGateway.Session.GetWorld().Checkpoint.SessionName.GetHashCode() + "_Config.xml");
                Data = MyAPIGateway.Utilities.SerializeFromXML<Data>(reader.ReadToEnd());
                reader.Close();
            }
            catch
            {
                Data = new Data();
                MyAPIGateway.Utilities.TryShowMessage("JimsCommands", "Failed to load data");
            }
        }

        public static void Save()
        {
            try
            {
                TextWriter writer = MyAPIGateway.Utilities.WriteFileInGlobalStorage("JimsCommands_" + MyAPIGateway.Session.GetWorld().Checkpoint.SessionName.GetHashCode() + "_Config.xml");
                writer.Write(MyAPIGateway.Utilities.SerializeToXML<Data>(Data));
                writer.Flush();
                writer.Close();
            }
            catch
            {
                MyAPIGateway.Utilities.TryShowMessage("JimsCommands", "Failed to save data");
            }
        }
    }

    /// <summary>
    /// Contains data/settings for the mod
    /// </summary>
    public class Data
    {
        public PermissionGroups Perms;

        public Data()
        {
            Perms = new PermissionGroups();
        }
    }

    /// <summary>
    /// Contains all the permission groups.
    /// </summary>
    public class PermissionGroups
    {
        public List<PermissionGroup> List;
        public PermissionGroup Default;

        public PermissionGroups()
        {
            List = new List<PermissionGroup>();
            List.Add(new PermissionGroup("all"));
            Default = null;
        }
    }

    /// <summary>
    /// Contains player IDs and commands accessible to those players.
    /// </summary>
    public class PermissionGroup
    {
        public string Name;
        public string Prefix;
        public List<IMyPlayer> Members;
        public List<ChatCommand> Commands;

        public PermissionGroup(string name, string prefix = "")
        {
            Name = name;
            Prefix = prefix;
            Members = new List<IMyPlayer>();
            Commands = new List<ChatCommand>();
        }
    }
}
