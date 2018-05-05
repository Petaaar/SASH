using System.IO;

namespace SASH.IO
{
    class Delete
    {
        private string path;

        /// <summary>
        /// Checks if a specified <paramref name="path"/> exists.
        /// </summary>
        /// <param name="path">The path specified.</param>
        /// <returns>boolean</returns>
        private static bool CheckPath(string path)
            => Directory.Exists(path);

        /// <summary>
        /// Deletes s file or many files in a specified or the current directory.
        /// </summary>
        /// <param name="arguments">An string array for the arguments needed.</param>
        public Delete(string path, string[] arguments)
        {
            this.path = path;

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
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = file;
            process.Start();
            process.WaitForExit(500); // wait a minute....
            if (!process.HasExited)
                process.Kill(); // *KILL IT!*

            process.Dispose();
        }
    }
}