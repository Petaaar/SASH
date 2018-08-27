using System;
using System.IO;
using SASH.IO;
using SASH.Hidden;
using SASH.OS;

//COUNT OF LINES:3000. Date:27.8.2018 г. 12:48:27. Stage: Added flexible os command functions.
namespace SASH
{
    /// <summary>
    /// The starting class of the program. CANNOT be inherited.
    /// </summary>
    sealed class Starter
    {
        #region Fields & Encapsulation

        private string path;

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
            if (!CheckPath(this.path)) Internal.Error($"The path \"{this.path}\" does not exist!");

            var commandArr = commandFull.Split(new char[] { ' ' }, StringSplitOptions.None);
            var commandInside = new System.Collections.Generic.List<string>();

            System.Threading.Tasks.Parallel.For(0, commandArr.Length,
                x => commandInside.Add(commandArr[x]));

            var command = commandInside[0];

            commandInside.Remove(command);

            switch (command.ToLower())
            {
                case "run":
                    new Run(this.path, commandInside.ToArray());
                    break;
                case "delete":
                    new Delete(this.path, commandInside.ToArray());
                    break;
                case "create":
                    new Create(this.path, commandInside.ToArray());
                    break;
                case "exit":
                    Internal.KillCmd();
                    break;
                case "clear":
                    try { Console.Clear(); Internal.Starter(this.path); }
                    catch (System.ComponentModel.Win32Exception) { Internal.KillCmd(); }
                    break;
                case "copy":
                    new Copy(this.path, commandInside.ToArray());
                    break;
                case "append":
                    new Append(this.path, commandInside[0]);
                    break;
                case "list":
                    try { new Lister(this.path, commandInside[0]); }
                    catch (System.IndexOutOfRangeException) { Internal.Starter(this.path); }
                    catch (System.ArgumentOutOfRangeException) { Internal.Starter(this.path); }
                    break;
                case "ls":
                    try { new Lister(this.path, commandInside[0]); }
                    catch (System.IndexOutOfRangeException) { Internal.Starter(this.path); }
                    catch (System.ArgumentOutOfRangeException) { Internal.Starter(this.path); }
                    break;
                case "cd":
                    ChangePath(commandInside[0]);
                    break;
                case "os":
                    new Os(this.path, commandInside.ToArray());
                    break;
                default:
                    Internal.Error($"Unrecognized command \"{command}\"!");
                    Internal.Starter(this.path);
                    break;
            }
        }

        private void ChangePath(string newPath)
        {
            try
            {
                if (!CheckPath(newPath))
                {
                    Internal.Error("This path does not exist or is invalid!");
                    Internal.Starter(this.path);
                }
                else
                    try { this.path = newPath; SavePath(this.path); }
                    catch (System.IO.IOException) { Internal.Error($"Invalid directory name \"{newPath}\"!"); Internal.Starter(this.path); }
                Internal.Starter(this.path);
            }
            catch (System.ComponentModel.Win32Exception) { Internal.Error($"Cannot change directory to \"{newPath}\"."); Internal.Starter(this.path); }
            catch (System.UnauthorizedAccessException) { Internal.Error($"Unauthorized access to \"{newPath}\"!"); Internal.Starter(this.path); }
        }

        /// <summary>
        /// Saves the boot path in c:\Users\Public\pathKeeper.txt"
        /// </summary>
        /// <param name="path">the boot path</param>
        private void SavePath(string path)
        {
            var currentPath = string.Empty;

            using (StreamReader reader = new StreamReader(@"C:\Users\Public\pathKeeper.txt"))
                 currentPath = reader.ReadLine();

            if (path == currentPath) this.path = path;

            using (StreamWriter writer = new StreamWriter(@"C:\Users\Public\pathKeeper.txt"))
                writer.WriteLine(path);


            this.path = path; //save the new path if new.
        }

        #endregion

        #region Public

        public string Path
        {
            get => path;
        }

        static void Main()
        {
            new Starter(@"C:\Users\petar\source\repos\Sash\Sash\files\");
        }


        public Starter(string path)
        {
            if (!File.Exists(@"C:\Users\Public\pathKeeper.txt"))
                File.Create(@"C:\Users\Public\pathKeeper.txt");
            
            Console.Write("SASH: ");
            var command = new GetCommand().ReadCommand();

            SavePath(path);

            Process(command);
        }

        #endregion

        ~Starter() => Internal.KillCmd();
    }
}