namespace Jimmacle.Commands
{
    using Sandbox.ModAPI;
    using System.Collections.Generic;

    /// <summary>
    /// Permissions controls
    /// </summary>
    class cmdPerms : ChatCommand
    {
        public cmdPerms() : base("Permissions", "perms", "Allows management of the permissions system.", "Usage: /perms <add/remove> <group/player/command> <item> [group]") { }

        public override string Invoke(List<string> parameters)
        {
            bool add;

            if (Storage.Data== null)
            {
                MyAPIGateway.Utilities.TryShowMessage("", "Created new storage instance");
                Storage.Data = new Data();
            }

            if (parameters.Count == 1)
            {
                string str = "";
                foreach (var g in Storage.Data.Perms.List)
                {
                    str += g.Name + ":\n- Commands:\n";
                    foreach (var c in g.Commands)
                    {
                        str += "- - " + c.Name + "\n";
                    }
                    str += "- Members:\n";
                    foreach (var m in g.Members)
                    {
                        str += "- - " + m.DisplayName + "\n";
                    }
                    str += "\n";
                }

                str += "\nAvailable Command Nodes:\n";
                foreach (var c in Logic.Commands)
                {
                    str += "- " + c.Name + "\n";
                }
                MyAPIGateway.Utilities.TryShowWindow("Permissions", str);
                return null;
            }

            if (parameters.Contains("add"))
            {
                add = true;
            }
            else if (parameters.Contains("remove"))
            {
                add = false;
            }
            else
            {
                return "Add/remove not specified";
            }

            if (parameters.Contains("group"))
            {
                if (parameters.Count < 4)
                {
                    return "Not enough parameters.";
                }

                PermissionGroup group = Storage.Data.Perms.List.Find(g => g.Name == parameters[3]);
                if (group == null && add == true)
                {
                    Storage.Data.Perms.List.Add(new PermissionGroup(parameters[3]));
                }
                else if (group != null && add == false)
                {
                    Storage.Data.Perms.List.Remove(group);
                }
            }
            if (parameters.Contains("command"))
            {
                if (parameters.Count < 5)
                {
                    return "Not enough parameters.";
                }

                ChatCommand command = Logic.Commands.Find(c => c.Name == parameters[3]);
                PermissionGroup group = Storage.Data.Perms.List.Find(g => g.Name == parameters[4]);
                if (command == null)
                {
                    return "Command not found.";
                }

                if (group == null)
                {
                    return "Group not found.";
                }

                if (add == true)
                {
                    group.Commands.Add(command);
                }
                else if (add == false)
                {
                    group.Commands.RemoveAll(c => c == command);
                }
            }
            if (parameters.Contains("player"))
            {
                if (parameters.Count < 5)
                {
                    return "Not enough parameters.";
                }

                List<IMyPlayer> players = new List<IMyPlayer>(); 
                MyAPIGateway.Multiplayer.Players.GetPlayers(players);
                IMyPlayer id = players.Find(p => p.DisplayName == parameters[3]);
                PermissionGroup group = Storage.Data.Perms.List.Find(g => g.Name == parameters[4]);
                if (id == null)
                {
                    return "Player not found.";
                }

                if (group == null)
                {
                    return "Group not found.";
                }

                if (add == true)
                {
                    group.Members.Add(id);
                }
                else if (add == false)
                {
                    group.Members.RemoveAll(i => i == id);
                }
            }

            return null;
        }
    }
}
