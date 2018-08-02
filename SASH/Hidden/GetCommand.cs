namespace SASH.Hidden
{   
    /// <summary>
    /// Represents a way to get a command from the STDIO.
    /// </summary>
    public struct GetCommand
    {
        public string ReadCommand()
        {
            string command = System.Console.ReadLine();

            if (command.ToLower() == "" || command == string.Empty)
                Internal.KillCmd();
            else
                return command;

            Internal.KillCmd(); //kill the CMD
            return ""; // just to have "return";
        }
    }
}