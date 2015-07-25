//This file contains the base class used to write commands
//

using System.Collections.Generic;
namespace Jimmacle.Commands
{
    public class ChatCommand
    {
        /// <summary>
        /// Full name of command; used in permissions.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The text the user must type to invoke the command.
        /// </summary>
        public string Command { get; private set; }

        /// <summary>
        /// A brief description of the command's function.
        /// </summary>
        public string ShortHelp { get; private set; }

        /// <summary>
        /// Full documentation of how to use the command and any subcommands.
        /// </summary>
        public string LongHelp { get; private set; }

        /// <summary>
        /// Contains the code executed when the command is invoked.
        /// </summary>
        /// <param name="parameters">Parts of the inputted command</param>
        /// <returns>Null on success, error message on fail.</returns>
        public virtual string Invoke(List<string> parameters)
        {
            return "";
        }

        public ChatCommand(string name, string command, string shortHelp = "No documentation found", string longHelp = "")
        {
            Name = name;
            Command = command;
            ShortHelp = shortHelp;
            LongHelp = longHelp == "" ? shortHelp : longHelp;
        }
    }
}
