namespace Jimmacle.Commands
{
    using Sandbox.ModAPI;
    using System;
    using System.IO;

    class Logger
    {
        /// <summary>
        /// Write a line of text to a file.
        /// </summary>
        /// <param name="name">Full file name</param>
        /// <param name="text">Text to write</param>
        public static void WriteLine(string name, string text)
        {
            string oldText;
            try
            {
                TextReader reader = MyAPIGateway.Utilities.ReadFileInGlobalStorage("JimsCommands_" + name);
                oldText = reader.ReadToEnd();
                reader.Close();
            }
            catch
            {
                oldText = "";
            }

            TextWriter writer = MyAPIGateway.Utilities.WriteFileInGlobalStorage("JimsCommands_" + name);

            DateTime now = DateTime.Now;
            string timestamp = "[" + now.ToShortDateString() + " " + now.TimeOfDay + "] ";

            writer.Write(oldText);
            writer.WriteLine(timestamp + text);
            writer.Flush();
            writer.Close();
        }
    }
}
