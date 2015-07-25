namespace Jimmacle.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Text.RegularExpressions;

    using Sandbox;
    using Sandbox.Common;
    using Sandbox.Definitions;
    using Sandbox.Game;
    using Sandbox.ModAPI;

    using VRage;
    using VRageMath;
    using Sandbox.Common.ObjectBuilders;

    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    class Logic : MySessionComponentBase
    {
        /// <summary>
        /// All available commands. If new commands are added they must be added to this list.
        /// </summary>
        public static List<ChatCommand> Commands = new List<ChatCommand>()
        {
            new cmdPerms(),
            new cmdAntigrief(),
        };

        private bool init = false;
        public static bool IsServer;
        public static bool IsOnline;

        /// <summary>
        /// Command parser
        /// </summary>
        /// <param name="messageText">Chat input</param>
        /// <param name="sendToOthers"></param>
        public static void MessageEntered(string messageText, ref bool sendToOthers)
        {
            string[] messageSplit = messageText.Split(' ');
            if (messageSplit.Length == 0)
                messageSplit = new string[] { messageText };

            if (messageText.StartsWith("/"))
            {

                //parse command into sections
                //
                var matches = Regex.Matches(messageText, "(\\w+|\".+\")");
                List<string> parameters = new List<string>();
                foreach (var m in matches)
                {
                    parameters.Add(m.ToString());
                }

                sendToOthers = false;

                //find command and try to execute it
                //
                ChatCommand command = Commands.Find(c => c.Command == parameters[0]);
                try
                {
                    if (command != null)
                    {
                        if (MyAPIGateway.Session.Player.CanAccess(command))
                        {
                            string result = command.Invoke(parameters);
                            if (result != null)
                            {
                                MyAPIGateway.Utilities.TryShowMessage("Error", result);
                            }
                        }
                        else
                        {
                            MyAPIGateway.Utilities.TryShowMessage("", "You don't have permission to do that.");
                        }
                    }
                    else
                    {
                        MyAPIGateway.Utilities.TryShowMessage("", "That command doesn't exist.");
                    }
                }
                catch (Exception ex)
                {
                    MyAPIGateway.Utilities.TryShowMessage(ex.GetType().ToString(), ex.Message);
                }
            }
        }

        /// <summary>
        /// Waits for session and initializes mod
        /// </summary>
        public override void UpdateBeforeSimulation()
        {
            //wait for session to finish loading
            //
            if (!init && MyAPIGateway.Session != null)
            {
                init = true;
                Storage.Load();

                //initialize depending on instance type
                //
                if (MyAPIGateway.Session.OnlineMode == MyOnlineModeEnum.OFFLINE)
                {
                    IsServer = true;
                    IsOnline = false;
                    MyAPIGateway.Utilities.MessageEntered += MessageEntered;
                    Logger.WriteLine("Log.txt", "Detected offline game");
                }
                else
                {
                    IsOnline = true;

                    //indicates dedicated server
                    //
                    if (MyAPIGateway.Session.Player == null)
                    {
                        IsServer = true;
                        MyAPIGateway.Multiplayer.RegisterMessageHandler(Network.SERVER_MSG_ID, Network.MessageHandler);
                        Logger.WriteLine("Log.txt", "Detected dedicated server");
                    }
                    //indicates host of game
                    //
                    else if (MyAPIGateway.Multiplayer.IsServerPlayer(MyAPIGateway.Session.Player.Client))
                    {
                        IsServer = true;
                        MyAPIGateway.Multiplayer.RegisterMessageHandler(Network.SERVER_MSG_ID, Network.MessageHandler);
                        MyAPIGateway.Multiplayer.RegisterMessageHandler(Network.CLIENT_MSG_ID, Network.MessageHandler);
                        MyAPIGateway.Utilities.MessageEntered += MessageEntered;
                        Logger.WriteLine("Log.txt", "Detected host of game");
                    }
                    //assume client
                    else
                    {
                        IsServer = false;
                        MyAPIGateway.Multiplayer.RegisterMessageHandler(Network.CLIENT_MSG_ID, Network.MessageHandler);
                        MyAPIGateway.Utilities.MessageEntered += MessageEntered;
                        Logger.WriteLine("Log.txt", "Detected client");
                    }
                }

                if (IsOnline && !IsServer)
                {
                    Network.SendMessage(new NetMessage(NetCommand.GetAll, "null"));
                    MyAPIGateway.Utilities.ShowMessage("", "Getting data from server");
                }
            }
        }


        /// <summary>
        /// Triggered on ingame save
        /// </summary>
        public override void SaveData()
        {
            Storage.Save();
            base.SaveData();
        }

        /// <summary>
        /// Unloads data on game exit
        /// </summary>
        protected override void UnloadData()
        {
            Storage.Save();
            MyAPIGateway.Utilities.MessageEntered -= MessageEntered;
            Logger.WriteLine("Log.txt", "Unloaded mod");
            base.UnloadData();
        }
    }
}
