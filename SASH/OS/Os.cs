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

        private void ParseArgs()
        {
            if (this.arguments != null && this.arguments.Length > 1)
            {
                if (this.arguments[0].Equals("env"))
                {
                    switch (arguments[1])
                    {
                        case "var": 
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
                }
            }
            else
            {
                Internal.Error($"Unrecognized OS arguments!");
            }
            
        }

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
                default:
                    Internal.Error($"Unrecognized OS argument \"{argument}\"!");
                    
                    break;
            }
        }
    }
}
