using SASH.Hidden;

namespace SASH.OS
{
    /// <summary>
    /// Modifies environment variables, threads and more.
    /// </summary>
    class Os
    {
        private readonly string path;

        private readonly string[] arguments;

        public Os(string path, string[] args)
        {
            this.path = path;

            if (args.Length == 1)
                ParseArgument(args[0]);
            else
            {
                this.arguments = args;
                ParseArgs();
            }
            Internal.Starter(this.path);
        }

        /// <summary>
        /// Parses multiple arguments within the command.
        /// </summary>
        private void ParseArgs()
        {
            if (this.arguments != null && this.arguments.Length > 1)
            {
                switch (arguments[0])
                {
                    case "env": //os env
                        {
                            switch (arguments[1])
                            {
                                case "var": //os env var
                                    {
                                        if (arguments[2] != "create")//os env var {VARNAME}
                                            Environments.GetEnvironmentVariableValue(arguments[2]);
                                        else // os env var create {VARNAME} {VALUE}
                                        {
                                            if (arguments[5] != null) // os env var create {VARNAME} {VALUE} {TARGET(default = user)}
                                                if (arguments[5] == "machine")
                                                    Environments.CreateEnvironmentVariable(arguments[3], arguments[4], System.EnvironmentVariableTarget.Machine);
                                                else if (arguments[5] == "process")
                                                    Environments.CreateEnvironmentVariable(arguments[3], arguments[4], System.EnvironmentVariableTarget.Process);
                                            Environments.CreateEnvironmentVariable(arguments[3], arguments[4]);
                                        }
                                        break;
                                    }
                                default:
                                    Internal.Error("Unrecognized environment request!");
                                    break;
                            }
                            break;
                        }
                    case "mkd"://os mkd DIRNAME
                        {
                            if (!System.String.IsNullOrEmpty(arguments[1]))
                            {
                                try
                                {
                                    new SASH.IO.Create(this.path, new string[] { "-d", arguments[1], "in", "path" });
                                } catch(System.IndexOutOfRangeException) { ; } //system security exception thrower..
                            }
                            else
                            {
                                Internal.Error("Invalid/null directory name!");
                            }
                            break;
                        }
                    case "show": //os show
                        {
                            if (!System.String.IsNullOrEmpty(arguments[1]))
                            {
                                if (arguments[1].Equals("this"))
                                    ParseArgument("-traverse");
                                else
                                {
                                    string arg = System.String.Empty;

                                    try
                                    {                                        
                                        if (System.String.IsNullOrEmpty(arguments[2]))
                                            arg = arguments[2];
                                    }
                                    catch (System.IndexOutOfRangeException) { ; }
                                    if (System.IO.Directory.Exists(arguments[1]))
                                        try
                                        {
                                            new IO.Lister(arguments[1], arg);
                                        }
                                        catch (System.UnauthorizedAccessException) { Internal.Error($"Unauthorized access to \"{arguments[1]}\""); }
                                }
                            }
                            else
                                Internal.Error("Too few arguments for \"os show\" command!");

                            break;
                        }
                    default:
                        Internal.Error($"Unrecognized OS arguments!");
                        break;
                }
            }
            else
            {
                Internal.Error($"Unrecognized OS arguments!");
            }
            
        }

        /// <summary>
        /// Parses a single <paramref name="argument"/>.
        /// </summary>
        /// <param name="argument">An argument to be parsed.</param>
        private void ParseArgument(string argument)
        {
            switch (argument)
            {
                case "-curpath":
                    System.Console.WriteLine($"Current path: {System.Environment.CurrentDirectory}"); break;
                case "-curthread":
                    System.Console.WriteLine($"Current thread id: {System.Environment.CurrentManagedThreadId}"); break;
                case "-bit":
                    System.Console.Write($"You are running ");
                    string bits = System.Environment.Is64BitOperatingSystem ? "64bit" : "32bit";
                    System.Console.WriteLine($"{bits} operating system!");
                    break;
                case "-traverse":
                    new IO.Lister(path, System.String.Empty);
                    break;
                default:
                    Internal.Error($"Unrecognized OS argument \"{argument}\"!");
                    
                    break;
            }
        }
    }
}
