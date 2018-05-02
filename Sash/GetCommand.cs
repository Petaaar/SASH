public struct GetCommand
{
    public string ReadCommand()
    {
        string command = System.Console.ReadLine();

        if (command.ToLower() == "" || command == string.Empty)
            Internal.KillCmd();
        else
            return $"{command}";

        Internal.KillCmd(); //kill the CMD
        return ""; // just to have "return";
    }
}