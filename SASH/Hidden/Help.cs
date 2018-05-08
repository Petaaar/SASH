namespace SASH.Hidden
{
    /// <summary>
    /// Displays a help information for a command.
    /// </summary>
    public struct Help
    {
        /// <summary>
        /// Contains short descriptions of all commands.
        /// </summary>
        private static readonly System.Collections.Generic.Dictionary<string, string>
            commandHelpShort = new System.Collections.Generic.Dictionary<string, string>
            {
                {"copy","=> Copies a file/directory in another/the boot directory."},
                {"create","=> Creates a new file(s) in a specified/the boot directory."},
                {"delete","=> Deletes a file(s)/directory in a specified/the boot directory"},
                {"run","=> Starts a new process to run an external application."},
                {"clear","=> Clears the console."},
                {"exit","\"Kills\" the program."}
            };

        /// <summary>
        /// Determines if a help for given command exists.
        /// </summary>
        /// <param name="command">The command.</param>
        private void Run(string command)
        {
            command = command.ToLower();

            if (command == "copy" || command == "create" || command == "delete" || command == "run")
                System.Console.WriteLine(commandHelpShort[command]);
            else Internal.Error($"Could not find help for command \"{command}\"!");
        }

        public Help(string command)
        {
            Run(command);
        }
    }
}