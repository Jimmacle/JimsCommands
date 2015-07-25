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

            if (Storage.Data == null)
            {
                MyAPIGateway.Utilities.TryShowMessage("", "Created new storage instance");
                Storage.Data = new Data();
            }

            if (parameters.Count == 1)
            {
                string str = "";
                foreach (var g in Storage.Data.Perms.Groups)
                {
                    str += g.Name + ":\n- Commands:\n";
                    foreach (var c in g.Commands)
                    {
                        str += "- - " + c + "\n";
                    }
                    str += "- Members:\n";
                    foreach (var m in g.Members)
                    {
                        str += "- - " + m.ToString() + "\n";
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

                PermissionGroup group = Storage.Data.Perms.Groups.Find(g => g.Name == parameters[3]);
                if (group == null && add == true && Storage.Data.Perms.Groups.Find(g => g.Name == parameters[3]) == null)
                {
                    Storage.Data.Perms.Groups.Add(new PermissionGroup(parameters[3]));
                }
                else if (group != null && add == false)
                {
                    Storage.Data.Perms.Groups.Remove(group);
                }
            }
            if (parameters.Contains("command"))
            {
                if (parameters.Count < 5)
                {
                    return "Not enough parameters.";
                }

                ChatCommand command = Logic.Commands.Find(c => c.Name == parameters[3]);
                PermissionGroup group = Storage.Data.Perms.Groups.Find(g => g.Name == parameters[4]);
                if (command == null)
                {
                    return "Command not found.";
                }

                if (group == null)
                {
                    return "Group not found.";
                }

                if (add == true && !group.Commands.Contains(command.Name))
                {
                    group.Commands.Add(command.Name);
                }
                else if (add == false)
                {
                    group.Commands.RemoveAll(c => c == command.Name);
                }
            }
            if (parameters.Contains("player"))
            {
                if (parameters.Count < 5)
                {
                    return "Not enough parameters.";
                }

                List<IMyIdentity> players = new List<IMyIdentity>();
                MyAPIGateway.Multiplayer.Players.GetAllIdentites(players);
                IMyIdentity id = players.Find(i => i.DisplayName == parameters[3]);
                PermissionGroup group = Storage.Data.Perms.Groups.Find(g => g.Name == parameters[4]);
                if (id == null)
                {
                    return "Player not found.";
                }

                if (group == null)
                {
                    return "Group not found.";
                }

                if (add == true && !group.Members.Contains(id.IdentityId))
                {
                    group.Members.Add(id.IdentityId);
                }
                else if (add == false)
                {
                    group.Members.RemoveAll(i => i == id.IdentityId);
                }
            }

            Network.SendMessage(new NetMessage(NetCommand.SyncPermissions, MyAPIGateway.Utilities.SerializeToXML<PermissionGroups>(Storage.Data.Perms)));
            return null;
        }
    }
}
