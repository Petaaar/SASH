using System;
using System.IO;
using SASH.IO;
using SASH.Hidden;

//2542 lines of code. Stage: not even close to finished. Date: 2.08.2018.

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
                    Environment.Exit(0);
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
                default:
                    Internal.Error($"Unrecognized command \"{command}\"!");
                    Internal.Starter(this.path);
                    break;
            }
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

            var command = new GetCommand().ReadCommand();

            SavePath(path);

            Process(command);
        }

        #endregion

        ~Starter() => Internal.KillCmd();
    }
}