using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace SASH
{
    /// <summary>
    /// The starting class of the program. CANNOT be inherited.
    /// </summary>
    sealed class Starter
    {
        #region Fields & Encapsulation

        private readonly string path;

        private string dest;

        #endregion

        #region Private

        /// <summary>
        /// Checks if a specified <paramref name="path"/> exists.
        /// </summary>
        /// <param name="path">The path specified.</param>
        /// <returns>boolean</returns>
        private static bool CheckPath(string path)
            => Directory.Exists(path);

        /// <summary>
        /// Determines whenever a string is a list of arguments needed for
        /// the <see cref="Create(string[])"/> function.
        /// </summary>
        /// <param name="argument"></param>
        /// <returns>boolean</returns>
        private static bool IsListOfFiles(string argument)
            => argument[0] == '{' & argument[argument.Length - 1] == '}';

        private static bool IsFileLocked(string path)
        {
            bool res = false;
            FileInfo info = new FileInfo(path);

            try
            {
                var stream = info.Open(FileMode.Open, FileAccess.ReadWrite);
                stream.Dispose();
            }
            catch (IOException) { res = true; }
            return res;
        }

        private static void TryKill(string file)
        {
            File.Delete(file);
            Process p = new Process();
            p.StartInfo.FileName = file;
            p.Start();
            p.WaitForExit(500); // wait a minute....
            if (!p.HasExited)
                p.Kill(); // *KILL IT!*
        }

        /// <summary>
        /// Processes a given <paramref name="commandFull"/> specified.
        /// </summary>
        /// <param name="commandFull">The command.</param>
        private void Process(string commandFull)
        {
            if (!CheckPath(this.path)) throw new ArgumentException(this.path);

            var commandArr = commandFull.Split(new char[] { ' ' }, StringSplitOptions.None);
            var commandInside = new List<string>();

            System.Threading.Tasks.Parallel.For(0, commandArr.Length,
                x => commandInside.Add(commandArr[x]));

            var command = commandInside[0];

            commandInside.Remove(commandInside[0]);

            switch (command)
            {
                case "run":
                    Run(commandInside.ToArray());
                    break;
                case "delete":
                    Delete(commandInside.ToArray());
                    break;
                case "create":
                    Create(commandInside.ToArray());
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                case "clear":
                    try { Console.Clear(); Internal.Starter(this.path); }
                    catch (System.ComponentModel.Win32Exception) { Internal.KillCmd(); }
                    break;
                default:
                    Internal.Error($"Unrecognized command \"{command}\"!");
                    Internal.Starter(this.path);
                    break;
            }
        }

#pragma warning disable CC0091 // Use static method
        /// <summary>
        /// Runs a program specified by the user, the process window could be 
        /// changed by the user. Usage of the command: [(programName)] (windowStyle).
        /// </summary>
        /// /// <param name="arguments">An string array for the arguments needed.</param>
        private void Run(string[] arguments)
#pragma warning restore CC0091 // Use static method
        {
            if (arguments.Length == 0)
                throw new ArgumentException("No arguments given. What to run!?");

            var programName = string.Empty;
            var windowStyle = string.Empty;

            if (arguments.Length == 2)
            {
                programName = arguments[0];
                windowStyle = arguments[1];
            }
            else if (arguments.Length == 1) programName = arguments[0];


            //run the program specified
            var process = new Process();
            process.StartInfo.FileName = programName;

            switch (windowStyle.ToLower())
            {
                case "normal":
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    break;
                case "minimized":
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                    break;
                case "maximized":
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                    break;
                case "hidden":
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    break;
                //if we don't have the argument or it's invalid
                default:
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    break;
            }

            try
            {
                process.Start();
            }
            catch (InvalidOperationException) { Internal.KillCmd(); }

            catch (System.ComponentModel.Win32Exception)
            { Internal.Error($"The program \"{programName}\" cannot be found or does not exists!"); }
            //dispose at the end
            process.Dispose();

            Internal.Starter(this.path);

        }

#pragma warning disable CC0091 //static method
#pragma warning disable CC0001 //use 'var' whenever possible
        /// <summary>
        /// Deletes s file or many files in a specified or the current directory.
        /// </summary>
        /// <param name="arguments">An string array for the arguments needed.</param>
        private void Delete(string[] arguments)
        {
            if (arguments.Length == 0)
                Internal.Error("No arguments given. What to run!?");
            if (arguments.Length <= 2 || arguments.Length > 3)
                Internal.Error("Too many or too few arguments given!");
            if (arguments[1] != "in")
                Internal.Error($"Expected keyword IN in the place of \"{arguments[1]}\".");

            var file = arguments[0];
            var destination = string.Empty;


            if (arguments[2] == nameof(this.path))
                destination = this.path;
            else
                destination = arguments[2];

            var dirInfo = new DirectoryInfo(destination);



            try
            {
                if (!CheckPath(destination))
                    Internal.Error($"The destination \"{destination}\" does not exist!");
                else if (dirInfo.GetFileSystemInfos().Length == 0)
                    Internal.Error($"The destination \"{destination}\" is empty!");
            }
            catch (FileNotFoundException)
            {
                Internal.Error($"The destination \"{destination}\" does not exist!");
                Internal.Starter(this.path);
            }

            if (file == "*")
            {
                try
                {
                    Console.WriteLine(path);
                    DeleteFullContent(destination);
                }
                catch (IOException)
                {
                    Internal.Error("Directory not found. Retrying..");
                    try { DeleteFullContent(destination); }
                    catch (IOException)
                    {
                        Internal.Error("FAILED! PLEASE RESTART!"); 
                    }
                    Internal.Starter(this.path);
                }
            }
            else
            {
                if (destination[destination.Length - 1] != @"\".ToCharArray()[0])
                    destination = destination + @"\";

                if (File.Exists(destination + file) && !IsFileLocked(destination + file))
                {
                    try
                    {
                        file = destination + file;
                        File.Delete(file);
                        Internal.Starter(this.path);
                    }
                    catch (IOException)
                    {
                        Internal.Error($"The file \"{file}\" is used by another program.");
                        TryKill(file);
                        Internal.Starter(this.path);
                    }
                }
                else
                {
                    if (destination[destination.Length - 1] != @"\".ToCharArray()[0])
                        destination += @"\";

                    File.Delete(file);
                    Internal.Starter(this.path);
                    Internal.Starter(this.path);
                }
            }

            Internal.Sleep(2500);
        }

        //restore the warnings

#pragma warning disable CA1822 // Mark members as static
        /// <summary>
        /// Creates a file / folder in the given in the <paramref name="arguments"/> directory.
        /// If there is not given any directory argument, the file is created in the current one.
        /// </summary>
        /// <param name="arguments">The arguments to the command.</param>
        private void Create(string[] arguments)
        {
            string filename = string.Empty;
            string destination = string.Empty;
            string[] filesArr = Array.Empty<string>();
            int argsLen = arguments.Length;

            if (argsLen == 1) filename = arguments[0];

            if (argsLen == 3 && arguments[1] == "in")
            {
                filename = arguments[0];
                if (Directory.Exists(arguments[2]) || arguments[2] == nameof(path))
                {
                    string argsLast = arguments[2];

                    if (argsLast[argsLast.Length - 1] == @"\".ToCharArray()[0])
                        destination = argsLast;
                    else
                        destination = argsLast + @"\";
                }
                else
                {
                    Internal.Error("Destination does not exist!");
                    //TODO: CREATING A NEW DESTINATION!!!
                    Internal.Starter(this.path);
                }
            }

            if (destination == nameof(path) || argsLen == 1)
                destination = this.path;

            if (IsListOfFiles(filename))
            {
                filesArr = filename.Split(new char[] { ',', '{', '}' }, StringSplitOptions.None);

                List<string> files = filesArr.ToList();

                //fixing the "path" issue.
                files.Remove(files[0]);
                files.Remove(files[files.Count - 1]);

                System.Threading.Tasks.Parallel.For(0, files.Count,
                    x => CreateFile(destination + files[x]));

                Internal.Starter(this.path);
            }
            else
            {
                if (File.Exists(destination + filename))
                {
                    Internal.Error("File already exists in the specified destination!");
                    Internal.Starter(this.path); //save from cross-thread operation!
                }
                else
                {
                    CreateFile(destination + filename);
                    Internal.Starter(this.path);
                }
            }

            if (argsLen == 1) CreateFile(destination + filename);

            Internal.Starter(this.path);

        }
#pragma warning restore
        /// <summary>
        /// Deletes EVERYTHING in a given <paramref name="destination"/>.
        /// </summary>
        /// <param name="destination">The destination.</param>
        private void DeleteFullContent(string destination)
        {
            var directoryInfo = new DirectoryInfo(destination);

            foreach (string item in Directory.GetFiles(destination))
                File.Delete(item);

            Internal.Starter(this.path);
        }

        private void DeleteContent()
        {
            DeleteFullContent(this.dest);
        }

        /// <summary>
        /// Creates a file with given <paramref name="filename"/>
        /// </summary>
        /// <param name="filename">The name of the file to be created.</param>
        private void CreateFile(string filename)
        {
            try
            {
                File.Create(filename);
                Console.WriteLine($"Created a new file in {filename}!");
                Internal.Starter(this.path);
            }
            catch (IOException) {; }
        }



        #endregion

        #region Public

        static void Main()
        {
            var s = new Starter(@"C:\Users\petar\source\repos\Sash\Sash\files\");
        }

        #endregion

        public Starter(string path)
        {
            var command = new GetCommand().ReadCommand();
            this.path = path;

            Process(command.ToLower());
        }

        ~Starter() => Internal.KillCmd();
    }
}