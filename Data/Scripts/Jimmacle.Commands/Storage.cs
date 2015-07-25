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
    using Sandbox.Common.ObjectBuilders;

    using VRage;
    using VRageMath;

    public static class Storage
    {
        public static Data Data;

        public static void Load()
        {
            if (Logic.IsOnline && !Logic.IsServer)
            {
                Logger.WriteLine("Log.txt", "Skipped local load");
                return;
            }

            try
            {
                TextReader reader = MyAPIGateway.Utilities.ReadFileInGlobalStorage("JimsCommands_" + MyAPIGateway.Session.GetWorld().Checkpoint.SessionName.GetHashCode() + "Config.xml");
                Data = MyAPIGateway.Utilities.SerializeFromXML<Data>(reader.ReadToEnd());
                if (Data == null)
                {
                    throw new Exception("Save data is null");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Data = new Data();
                MyAPIGateway.Utilities.TryShowMessage("Error", "Failed to load data");
                Logger.WriteLine("Errors.txt", ex.GetType().ToString() + ": " + ex.Message);
            }
        }

        public static void Save()
        {
            if (Logic.IsOnline && !Logic.IsServer)
            {
                Logger.WriteLine("Log.txt", "Skipped local save");
                return;
            }

            try
            {
                TextWriter writer = MyAPIGateway.Utilities.WriteFileInGlobalStorage("JimsCommands_" + MyAPIGateway.Session.GetWorld().Checkpoint.SessionName.GetHashCode() + "Config.xml");
                writer.Write(MyAPIGateway.Utilities.SerializeToXML<Data>(Data));
                writer.Flush();
                writer.Close();
            }
            catch (Exception ex)
            {
                MyAPIGateway.Utilities.TryShowMessage("Error", "Failed to save data");
                Logger.WriteLine("Errors.txt", ex.GetType().ToString() + ": " + ex.Message);
            }
        }
    }

    /// <summary>
    /// Contains data/settings for the mod
    /// </summary>
    public class Data
    {
        string SessionName = MyAPIGateway.Session.GetWorld().Checkpoint.SessionName;
        public PermissionGroups Perms;
        public GridInfo Grids;

        public Data()
        {
            MyAPIGateway.Utilities.TryShowMessage("", "Generating new mod data");
            Perms = new PermissionGroups();
            Grids = new GridInfo();
        }
    }

    /// <summary>
    /// Permission node
    /// </summary>
    public class PermissionGroup
    {
        public string Name;
        public string Prefix;
        public List<long> Members;
        public List<string> Commands;

        public PermissionGroup(string name, string prefix = "")
        {
            Name = name;
            Prefix = prefix;
            Members = new List<long>();
            Commands = new List<string>();
        }

        public PermissionGroup()
        {
            Name = "";
            Prefix = "";
            Members = new List<long>();
            Commands = new List<string>();
        }
    }

    public class PermissionGroups
    {
        public List<PermissionGroup> Groups;

        public PermissionGroups()
        {
            Groups = new List<PermissionGroup>();
        }
    }

    /// <summary>
    /// Custom grid information
    /// </summary>
    public class Grid
    {
        public long Id;
        public bool Safe;

        public Grid(long id)
        {
            Id = id;
            Safe = false;
        }

        public Grid()
        {
            Id = 0;
            Safe = false;
        }
    }

    public class GridInfo
    {
        public List<Grid> Grids;

        public GridInfo()
        {
            Grids = new List<Grid>();
        }
    }
}
