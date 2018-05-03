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
                    Create();
                    break;
                case "exit":
                    Internal.KillCmd();
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

            switch (windowStyle)
            {
                case "normal": process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    break;
                case "minimized": process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                    break;
                case "maximized": process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                    break;
                case "hidden": process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    break;
                    //if we don't have the argument or it's invalid
                default: process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
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
            var destination = arguments[2];
            

            if (destination == nameof(this.path)) destination = this.path;
            var dirInfo = new DirectoryInfo(destination);

            try
            {
                if (!CheckPath(destination))
                    Internal.Error($"The destination \"{destination}\" does not exist!");
                else if (dirInfo.GetFileSystemInfos().Length == 0)
                    Internal.Error($"The destination \"{destination}\" is empty!");
            } catch (FileNotFoundException)
            {
                Internal.Error($"The destination \"{destination}\" does not exist!");
                Internal.Starter(this.path);
            }

            if (file == "*")
            {
                try
                {
                    DeleteFullContent(destination);
                }
                catch (IOException) { Internal.Error("Directory not found! TERMINATING!"); }
            }
            else
            {
                if (File.Exists(destination + @"\" + file))
                {
                    file = destination + @"\" + file;
                    File.Delete(file);
                }
                else
                {
                    Internal.Error($"File \"{file}\" does not exists in directory \"{destination}\"!");
                    Internal.Starter(this.path);
                }
            }

            System.Threading.Thread.Sleep(2500);
        }
//restore the warnings
#pragma warning restore 

        private void Create()
        {

        }

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

        #endregion

        #region Public

        static void Main()
        {
            var s = new Starter(@"C:\Users\petar\source\repos\Sash\Sash\files");
        }

        #endregion

        public Starter(string path)
        {
            var command = new GetCommand().ReadCommand();
            this.path = path;

            Process(command);
        }

        ~Starter() => Internal.KillCmd();
    }
}
