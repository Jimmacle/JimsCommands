namespace Jimmacle.Commands
{
    using Sandbox.ModAPI;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Permissions controls
    /// </summary>
    class cmdPerms : ChatCommand
    {
        public cmdPerms() : base("Permissions", "perms", "Allows management of the permissions system.", 
            "This command gives access to the permissions system.\n\n" +
            "/perms <add/remove> <group> <groupname>\n" +
            "/perms <add/remove> <player> <playername> <groupname>\n" +
            "/perms <add/remove> <command> <commandname> <groupname>\n\n" +
            "/perms list: list current setup") { }

        public override string Invoke(List<string> parameters)
        {
            if (parameters.Count == 1)
            {
                MyAPIGateway.Utilities.TryShowWindow(this.Name, this.LongHelp);
                return null;
            }

            //get all the needed information from the command
            //
            bool add = true;
            
            //decide whether to add or remove
            //
            if (parameters[1] == "add")
            {
                add = true;
            }
            else if (parameters[1] == "remove")
            {
                add = false;
            }
            else if (parameters[1] == "list")
            {
                ListPerms();
                return null;
            }

            //decide what to add or remove
            //
            string type = parameters[2];
            string targetName = parameters[3];
            string groupName;

            switch (type)
            {
                case "group":
                    return EditGroups(add, targetName);

                case "player":
                    groupName = parameters[4];
                    return EditPlayers(add, targetName, groupName);

                case "command":
                    groupName = parameters[4];
                    return EditCommands(add, targetName, groupName);
            }

            //save locally or sync with other players (logic is in the called methods)
            //
            Storage.Save();
            Network.SendMessage(new NetMessage(NetCommand.SyncPermissions, MyAPIGateway.Utilities.SerializeToXML<PermissionGroups>(Storage.Data.Perms)));
            return null;
        }

        private string EditGroups(bool add, string targetName)
        {
            if (add)
            {
                if (Storage.Data.Perms.Groups.Find(g => g.Name == targetName) == null)
                {
                    Storage.Data.Perms.Groups.Add(new PermissionGroup(targetName));
                }
                else
                {
                    return "Group already exists";
                }
            }
            else
            {
                if (Storage.Data.Perms.Groups.Find(g => g.Name == targetName) == null)
                {
                    return "Group doesn't exist";
                }
                else
                {
                    Storage.Data.Perms.Groups.RemoveAll(g => g.Name == targetName);
                }
            }
            return null;
        }

        private string EditPlayers(bool add, string targetName, string groupName)
        {
            PermissionGroup group = Storage.Data.Perms.Groups.Find(g => g.Name == groupName);
            IMyIdentity identity = Utilities.GetIdentity(targetName);

            if (add)
            {
                if (group.Members.Contains(identity.IdentityId))
                {
                    return "Player is already in group";
                }
                else
                {
                    group.Members.Add(identity.IdentityId);
                }
            }
            else
            {
                if (group.Members.Contains(identity.IdentityId))
                {
                    group.Members.Remove(identity.IdentityId);
                }
                else
                {
                    return "Player is not in group";
                }
            }
            return null;
        }

        private string EditCommands(bool add, string targetName, string groupName)
        {
            PermissionGroup group = Storage.Data.Perms.Groups.Find(g => g.Name == groupName);
            ChatCommand command = Logic.Commands.Find(c => c.Name == targetName);

            if (add)
            {
                if (group.Commands.Contains(command.Name))
                {
                    return "Command is already in group";
                }
                else
                {
                    group.Commands.Add(command.Name);
                }
            }
            else
            {
                if (group.Commands.Contains(command.Name))
                {
                    group.Commands.Remove(command.Name);
                }
                else
                {
                    return "Command is not in group";
                }
            }
            return null;
        }

        private void ListPerms()
        {
            string body = "";

            foreach(var group in Storage.Data.Perms.Groups)
            {
                List<string> names = new List<string>();
                foreach (long id in group.Members)
                {
                    names.Add(Utilities.GetIdentity(id).DisplayName);
                }
                body += String.Format("{0}:\n- Members:\n- - {1}\n- Commands:\n- - {2}\n\n", group.Name, String.Join("\n- - ", names), String.Join("\n- - ", group.Commands));
            }

            MyAPIGateway.Utilities.TryShowWindow("Permissions", body);
        }
    }
}
