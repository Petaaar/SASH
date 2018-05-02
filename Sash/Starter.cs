﻿using System;
using System.IO;
using System.Diagnostics;

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

        private bool CheckPath(string path)
            => Directory.Exists(path);

        private void Process(string command)
        {
            if (!CheckPath(this.path)) throw new ArgumentException(this.path);

            switch (command)
            {
                case "run": Run();
                    break;
                case "delete": Delete();
                    break;
                case "create": Create();
                    break;
                default: Internal.Error($"Unrecognized command \"{command}\"!");
                    System.Threading.Thread.Sleep(1000);
                    break;
            }
        }

#pragma warning disable CC0091 // Use static method
        /// <summary>
                              /// Runs a program specified by the user, the process window could be 
                              /// changed by the user. Usage of the command: [(programName)] (windowStyle).
                              /// </summary>
        private void Run()
#pragma warning restore CC0091 // Use static method
        {
            Console.Write("Program name:"); var name = Console.ReadLine();
            var programName = string.Empty;
            var windowStyle = string.Empty;

            // get any arguments to the command
            if (name.Contains(" "))
            {
                int index = 0;

                for (int i = 0; i < name.Length; i++)
                    if (name[i] == ' ')
                        index = i; //set the current index.

                var pname = new System.Text.StringBuilder();//program name
                var wstyle = new System.Text.StringBuilder(); //window style

                pname.Append(programName);
                wstyle.Append(windowStyle);

                for (int i = 0; i < index; i++) //get the program name!
                    pname.Append(name[i]);
                programName = pname.ToString();

                for (int i = index + 1; i < name.Length; i++) //get the argument.
                    wstyle.Append(name[i]);
                windowStyle = wstyle.ToString();
            }
            else programName = name;
            
           
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
            process.WaitForExit();
            //dispose at the end
            process.Dispose();
        }

        /// <summary>
        /// Deletes s file or many files in a specified or the current directory.
        /// </summary>
        private void Delete()
        {
            Console.WriteLine($"Current path specified:{this.path}");
            Console.Write("Delete it's content? [Y/N]:");
            var yn = Console.ReadLine().ToLower().ToCharArray()[0];

            if (yn == 'y' || yn == 'n')
            {
                if (yn == 'y')
                {
                    Console.Write("Are you sure? This will delete all files in the directory!:");
                    var yes = Console.ReadLine();
                    if (yes == "yes" || yes.ToCharArray()[0] == 'y')
                        DeleteContent();
                }
            }
            else Internal.Error("EXPECTED Y/N VALUE!");

            System.Threading.Thread.Sleep(1000);
        }

        private void Create()
        {

        }

        private void DeleteContent()
        {
            foreach (string item in Directory.GetFiles(this.path))
            {
                File.Delete(item);
            }
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

            Process(command);
        }
        
        ~Starter() => Array.ForEach(System.Diagnostics.Process.GetProcessesByName("cmd"),
                x => x.Kill());

        public static void Kill()
            => Array.ForEach(System.Diagnostics.Process.GetProcessesByName("cmd"),
                    x => x.Kill());
    }
}
