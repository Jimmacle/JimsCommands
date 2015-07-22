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
    using Sandbox.Common.ObjectBuilders;

    public static class Extensions
    {
        /// <summary>
        /// Try showing a message without crashing a dedicated instance.
        /// </summary>
        /// <param name="util">Utilities instance</param>
        /// <param name="sender">String to appear as the sender of the message</param>
        /// <param name="messageText">String to appear as the message</param>
        /// <returns>Successfully showed message?</returns>
        public static bool TryShowMessage(this IMyUtilities util, string sender, string messageText)
        {
            if (MyAPIGateway.Session.Player != null)
            {
                util.ShowMessage(sender, messageText);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Show a window with a title and body.
        /// </summary>
        /// <param name="util"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static bool TryShowWindow(this IMyUtilities util, string title, string body)
        {
            if (MyAPIGateway.Session.Player != null)
            {
                util.ShowMissionScreen(title, null, null, body);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a byte array containing this string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            for (int i = 0; i < str.Length; i++)
            {
                bytes[i] = (byte)str[i];
            }
            return bytes;
        }

        /// <summary>
        /// Returns a string built from this byte array.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetString(this byte[] bytes)
        {
            string str = "";
            foreach (byte b in bytes)
            {
                str += (char)b;
            }
            return str;
        }

        /// <summary>
        /// Checks if a player can use a certain command based on the permissions system.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static bool CanAccess(this IMyPlayer player, ChatCommand command)
        {
            if (player.IsAdmin())
                return true;
            if (Storage.Data.Perms.List.Count == 0)
                return true;
            if (Storage.Data.Perms.List.Find(g => g.Members.Contains(player) && g.Commands.Contains(command)) != null)
                return true;

            return false;
        }

        /// <summary>
        /// Returns if the player is an admin on the server.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool IsAdmin(this IMyPlayer player)
        {
            if (MyAPIGateway.Session.OnlineMode == MyOnlineModeEnum.OFFLINE || MyAPIGateway.Multiplayer.IsServerPlayer(player.Client))
                return true;

            var clients = MyAPIGateway.Session.GetCheckpoint("null").Clients;
            if (clients != null)
            {
                var client = clients.Find(c => c.SteamId == player.SteamUserId && c.IsAdmin);
                return client != null;
            }
            return false;
        }
    }
}
