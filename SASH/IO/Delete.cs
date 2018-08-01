using System.IO;
using SASH.Hidden;

namespace SASH.IO
{
    class Delete
    {
        #region Private

        private string path;

        /// <summary>
        /// Checks if a specified <paramref name="path"/> exists.
        /// </summary>
        /// <param name="path">The path specified.</param>
        /// <returns>boolean</returns>
        private static bool CheckPath(string path)
            => Directory.Exists(path);

        /// <summary>
        /// Deletes all files in a certain path.
        /// </summary>
        /// <param name="path">The path.</param>
        private void DeleteAllInPath(string path)
        {
            try
            {
                DeleteFullContent(path);
            }
            catch (IOException)
            {
                Internal.Error("Directory not found. Retrying..");
                try { DeleteFullContent(path); }
                catch (IOException)
                {
                    Internal.Error("FAILED! PLEASE RESTART!");
                }
                Internal.Starter(this.path);
            }
        }

        /// <summary>
        /// Deletes a single <paramref name="file"/> in a specified <paramref name="destination"/>.
        /// </summary>
        /// <param name="file">The file name.</param>
        /// <param name="destination">The full path for the file.</param>
        private void DeleteSingleFile(string file, string destination)
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
            catch (System.NotSupportedException)
            {
                Internal.Error($"Could not delete \"{file}\"; Not supported :(");
                Internal.Starter(this.path);
            }
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

        /// <summary>
        /// Checks if a file is locked or used by another process.
        /// </summary>
        /// <param name="path">Path for the file to check.</param>
        /// <returns>boolean</returns>
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

        /// <summary>
        /// Tries to kill every process using a certain <paramref name="file"/>.
        /// </summary>
        /// <param name="file">The file to look for.</param>
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

        /// <summary>
        /// Deletes every file that contains a specific "<paramref name="like"/>" argument.
        /// EXAMPLE: delete * in path where test -> "test" must be contained in every file in order it to be deleted.
        /// </summary>
        /// <param name="path">The path to look for.</param>
        /// <param name="like">The specific argument.</param>
        private void DeleteEverythingWhereContained(string path, string like)
        {
            if (CheckPath(path))
            {
                if (like != null || like != string.Empty || like != " ")
                {
                    var dirItems = Directory.GetFiles(path);

                    var commonItems = new System.Collections.Generic.List<string>();

                    if (dirItems.Length != 0) //empty directory
                    {
                        foreach (string item in dirItems)
                            if (Path.GetFileNameWithoutExtension(item).Contains(like))
                                commonItems.Add(item);
                    }
                    else Internal.Error($"The directory {path} is empty!");

                    if (commonItems.Count == 0) Internal.Error($"No items with common sign \"{like}\" were found in {path}!");
                    // at least 1 common item
                    else
                    {
                        System.Console.WriteLine("MATCHING FILES FOUND!");

                        Internal.Ask($"Show all {commonItems.Count} matching files?[Y/N]:"); char yn = System.Console.ReadLine().ToLower().ToCharArray()[0];

                        if (yn == 'y') //if you want to see all the items
                        {
                            System.Console.WriteLine("FILES TO DELETE:");
                            foreach (string item in commonItems)
                                System.Console.WriteLine(item);
                        }

                        Internal.Ask("Are you sure you want to delete all these items?:[Y/N]"); char y = System.Console.ReadLine().ToLower().ToCharArray()[0];

                        if (y == 'y')
                        {
                            foreach (string item in commonItems)
                                File.Delete(item);
                        }
                    }
                }
                else Internal.Error("Empty \"like\" argument!");
            }
            else Internal.Error("The path is incorrect or does not exist!");

            Internal.Starter(this.path);
        }

        /// <summary>
        /// Deletes every file with name, matching the <paramref name="matchSign"/> argument.
        /// </summary>
        /// <param name="path">The full path to file to look for.</param>
        /// <param name="matchSign">A sign to look for.</param>
        private void DeleteEverythingWhereMatches(string path, string matchSign)
        {
            if (CheckPath(path))
            {
                if (matchSign != string.Empty || matchSign != " " || matchSign != null)
                {
                    var dirItems = Directory.GetFiles(path);

                    var commonItems = new System.Collections.Generic.List<string>();

                    if (dirItems.Length != 0) //empty directory
                    {
                        foreach (string item in dirItems)
                            if (Path.GetFileNameWithoutExtension(item) == matchSign)
                                commonItems.Add(item);
                    }
                    else Internal.Error($"The directory {path} is empty!");

                    if (commonItems.Count == 0) Internal.Error($"No items with matching sign \"{matchSign}\" were found in {path}!");
                    // at least 1 common item
                    else
                    {
                        System.Console.WriteLine("MATCHING FILES FOUND!");


                        Internal.Ask($"Show all {commonItems.Count} matching files?[Y/N]:"); char yn = System.Console.ReadLine().ToLower().ToCharArray()[0];

                        if (yn == 'y') //if you want to see all the items
                        {
                            System.Console.WriteLine("FILES TO DELETE:");
                            foreach (string item in commonItems)
                                System.Console.WriteLine(item);
                        }

                        Internal.Ask("Are you sure you want to delete all these items?:[Y/N]"); char y = System.Console.ReadLine().ToLower().ToCharArray()[0];

                        if (y == 'y')
                        {
                            foreach (string item in commonItems)
                                File.Delete(item);
                        }
                    }
                }
                else Internal.Error("Empty \"matchSign\" argument!");
            }
            else Internal.Error("The path is incorrect or does not exist!");

            Internal.Starter(this.path);
        }

        #endregion

        /// <summary>
        /// Deletes s file or many files in a specified or the current directory.
        /// </summary>
        /// <param name="arguments">An string array for the arguments needed.</param>
        public Delete(string path, string[] arguments)
        {
            this.path = path;

            if (arguments.Length == 0)
            {
                Internal.Error("Empty arguments!");
                Internal.Starter(this.path);
            }
            if ((arguments.Length <= 2 || arguments.Length > 6) && arguments[0] != "-h")
                Internal.Error("Too many or too few arguments given!");
            if (arguments.Length == 1 && arguments[0] == "-h")
            {
                new Help("delete");
                Internal.Starter(this.path);
            }
            else Internal.Error("Too few arguments given!");
            if (arguments[1] != "in")
                Internal.Error($"Expected keyword IN in the place of \"{arguments[1]}\".");
            if (arguments.Length == 5 && arguments[3] != "where")
                Internal.Error($"Expected keyword WHERE in the place of \"{arguments[3]}\".");
            if (arguments.Length == 6 && arguments[5] == string.Empty)
                Internal.Error($"Expected non-empty argument after \"{arguments[4]}\".");


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

            if (file == "*" && arguments.Length == 3)
                DeleteAllInPath(destination);
            //delete file
            else if (arguments.Length == 2)
            {
                if (destination[destination.Length - 1] != @"\".ToCharArray()[0])
                    destination = destination + @"\";

                if (File.Exists(destination + file) && !IsFileLocked(destination + file))
                {
                    DeleteSingleFile(file, destination);
                }
                else
                {
                    if (destination[destination.Length - 1] != @"\".ToCharArray()[0])
                        destination += @"\";

                    File.Delete(file);
                    Internal.Starter(this.path);
                }

                Internal.Starter(this.path);
            }

            //delete file in path
            else if (arguments.Length == 3)
            {
                file = arguments[0];
                if (arguments[2] == " " || arguments[2] == string.Empty) destination = nameof(path);
                else destination = arguments[2];

                DeleteSingleFile(file, path);

            }

            else if (arguments.Length == 5 && file == "*")
                DeleteEverythingWhereContained(destination, arguments[4]);
            

            else if (arguments.Length == 6 && (file == "*" && arguments[4] == "is"))
                DeleteEverythingWhereMatches(destination, arguments[5]);

            Internal.Starter(this.path);
        }
    }
}