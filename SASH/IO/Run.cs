﻿using System;
using SASH.Hidden;
using System.Threading;
using System.Diagnostics;

namespace SASH.IO
{
    /// <summary>
    /// Starts a new file/website/application window.
    /// </summary>
    public struct Run
    {
        private string path;

        /// <summary>
        /// Runs a program specified by the user, the process window could be 
        /// changed by the user. Usage of the command: [(programName)] (windowStyle).
        /// </summary>
        /// /// <param name="arguments">An string array for the arguments needed.</param>
        public Run(string path,string[] arguments)
        {
            this.path = path;

            if (arguments.Length == 0)
                throw new ArgumentException("No arguments given. What to run!?");

            var programName = string.Empty;
            var windowStyle = string.Empty;

            if (arguments.Length == 2)
            {
                programName = arguments[0];
                windowStyle = arguments[1];
            }

            else if (arguments.Length == 1 && arguments[0] != "-h") programName = arguments[0];
            else if (arguments.Length == 1 && arguments[0] == "-h") new Help("run");
            else Internal.Error("Too much arguments given!");

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
                if (arguments[0] != "-h")
                    process.Start();
            }
            catch (InvalidOperationException) { Internal.Error($"Could not start the program {programName}."); }

            catch (System.ComponentModel.Win32Exception)
            { Internal.Error($"The program \"{programName}\" cannot be found or does not exists!"); }
            //dispose at the end
            process.Dispose();

            Internal.Starter(this.path);

        }
    }
}