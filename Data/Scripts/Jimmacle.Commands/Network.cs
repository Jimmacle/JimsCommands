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

    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class Network : MySessionComponentBase
    {
        public const ushort CLIENT_MSG_ID = 1337;
        public const ushort SERVER_MSG_ID = 7331;

        /// <summary>
        /// Deals with incoming messages
        /// </summary>
        /// <param name="message">Incoming data</param>
        public static void MessageHandler(byte[] message)
        {
            string str = Encoding.ASCII.GetString(message);
            Logger.WriteLine("Log.txt", "Got network message");
            Logger.WriteLine("Log.txt", Encoding.ASCII.GetString(message));
            NetMessage msg = MyAPIGateway.Utilities.SerializeFromXML<NetMessage>(str);
            switch (msg.Command)
            {
                case NetCommand.SyncAll:
                    Storage.Data = MyAPIGateway.Utilities.SerializeFromXML<Data>(msg.Message);
                    break;

                case NetCommand.SyncPermissions:
                    Storage.Data.Perms = MyAPIGateway.Utilities.SerializeFromXML<PermissionGroups>(msg.Message);
                    break;

                case NetCommand.SyncGridData:
                    Storage.Data.Grids = MyAPIGateway.Utilities.SerializeFromXML<GridInfo>(msg.Message);
                    break;

                case NetCommand.GetAll:
                    Network.SendMessage(new NetMessage(NetCommand.SyncAll, MyAPIGateway.Utilities.SerializeToXML<Data>(Storage.Data)));
                    break;

                default:
                    Logger.WriteLine("Log.txt", "Unknown net message recieved, discarding");
                    break;
            }

            if (Logic.IsServer) MyAPIGateway.Multiplayer.SendMessageTo(SERVER_MSG_ID, message, CLIENT_MSG_ID);
        }

        /// <summary>
        /// Sends a NetMessage to a specific client.
        /// </summary>
        /// <param name="id">ID of the recipient</param>
        /// <param name="msg">NetMessage to send</param>
        public static void SendMessage(NetMessage msg)
        {
            string serialized = MyAPIGateway.Utilities.SerializeToXML<NetMessage>(msg);
            if (Logic.IsServer)
            {
                Logger.WriteLine("Log.txt", "Sending message to others");
                MyAPIGateway.Multiplayer.SendMessageToOthers(CLIENT_MSG_ID, Encoding.ASCII.GetBytes(serialized));
            }
            else
            {
                Logger.WriteLine("Log.txt", "Sending message to server");
                MyAPIGateway.Multiplayer.SendMessageToServer(SERVER_MSG_ID, Encoding.ASCII.GetBytes(serialized));
            }
        }

        /// <summary>
        /// Sends a NetMessage to the server.
        /// </summary>
        /// <param name="msg">NetMessage to send</param>
        public static void SendMessageToServer(NetMessage msg)
        {
            string serialized = MyAPIGateway.Utilities.SerializeToXML<NetMessage>(msg);
            MyAPIGateway.Multiplayer.SendMessageTo(CLIENT_MSG_ID, Encoding.ASCII.GetBytes(serialized), SERVER_MSG_ID);
        }
    }

    /// <summary>
    /// Defines types of network messages.
    /// </summary>
    public enum NetCommand
    {
        //Sync the whole data instance
        SyncAll,

        //Sync permissions
        SyncPermissions,

        //Sync grid information
        SyncGridData,

        //Get config on connect
        GetAll,
    }

    /// <summary>
    /// A network message.
    /// </summary>
    public class NetMessage
    {
        public NetCommand Command;
        public string Message;

        public NetMessage(NetCommand command, string message)
        {
            Command = command;
            Message = message;
        }

        public NetMessage()
        {
            Command = NetCommand.GetAll;
            Message = "";
        }
    }
}
