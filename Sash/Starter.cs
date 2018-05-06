using System;
using System.IO;
using SASH.IO;
using SASH.Hidden;

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
                default:
                    Internal.Error($"Unrecognized command \"{command}\"!");
                    Internal.Starter(this.path);
                    break;
            }
        }
        
        #endregion

        #region Public

        static void Main()
        {
            new Starter(@"C:\Users\petar\source\repos\Sash\Sash\files\");
        }

        
        public Starter(string path)
        {
            var command = new GetCommand().ReadCommand();
            this.path = path;

            Process(command);
        }

        #endregion

        ~Starter() => Internal.KillCmd();
    }
}