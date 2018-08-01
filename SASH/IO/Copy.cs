using SASH.Hidden;
using System.IO;

namespace SASH.IO
{
    /// <summary>
    /// Class, responsible for copying a files to/from location.
    /// </summary>
    class Copy
    {
        #region Private

        /// <summary>
        /// The "boot" path of the program.
        /// </summary>
        private readonly string path;

        /// <summary>
        /// Copies a single <paramref name="file"/> in the boot path.
        /// </summary>
        /// <param name="file">File to copy.</param>
        private void CopySingleFile(string file)
        {
            if (file != string.Empty && file != " ")
            {
                try
                {
                    File.Copy(file, Path.GetFileName(file));
                }
                catch (IOException) { Internal.Error($"File already exists in path \"{this.path}\"."); }
            }
            else Internal.Error("Empty file argument!");

            Internal.Starter(this.path);
        }

        /// <summary>
        /// Copies a single <paramref name="file"/> into a specified path(<paramref name="pathTo"/>).
        /// </summary>
        /// <param name="file">The file to copy.</param>
        /// <param name="pathTo">A path to where to copy the file.</param>
        private void CopySingleFile(string file,string pathTo)
        {
            if (file != string.Empty && file != " ")
            {
                if (pathTo != string.Empty && pathTo != " ")
                {
                    try
                    {
                        if (File.Exists(Path.Combine(Path.GetFileName(file),file)))
                            if (Directory.Exists(pathTo))
                                File.Copy(file, pathTo + Path.GetFileName(file));
                            else Internal.Error($"Directory \"{pathTo}\" does not exist!");
                        else Internal.Error($"File path \"{file}\" does not exist!");
                    }
                    catch (IOException e) { Internal.Error($"EXCEPTION:{e.Message}!"); }
                }
                else Internal.Error("Empty path argument!");
            }
            else Internal.Error("Empty file argument!");

            Internal.Starter(this.path);
        }

        /// <summary>
        /// Copies a <paramref name="directory"/> in the boot one. RECURSIVE.
        /// </summary>
        /// <param name="directory">Directory to copy.</param>
        private void CopyDir(string directory)
        {
            if (directory != string.Empty && directory != " ")
            {
                var files = System.Array.Empty<string>();

                if (!Directory.Exists(directory))
                    Internal.Error($"Directory \"{directory}\" does not exist!");
                files = Directory.GetFileSystemEntries(directory);

                foreach (string subElement in files)
                { 
                    if (Directory.Exists(subElement))
                        CopyDir(subElement);
                    else if (!File.Exists(this.path + Path.GetFileName(subElement)))
                    {
                        try
                        {
                            File.Copy(subElement, this.path + Path.GetFileName(subElement));
                        }
                        catch (IOException e) { Internal.Error($"AN ERROR OCCURRED:{e.Message}"); }
                    }
                }
            }
            else Internal.Error("Empty directory argument!");

            Internal.Starter(this.path);
        }

        /// <summary>
        /// Copies a <paramref name="directory"/> 
        /// in another specified <paramref name="newDirectory"/>. RECURSIVE!
        /// </summary>
        /// <param name="directory">Directory to be copied.</param>
        /// <param name="newDirectory">Directory to copy the <paramref name="directory"/> to.</param>
        private void CopyDir(string directory, string newDirectory)
        {
            if (directory != " " && directory != string.Empty)
            {
                if (newDirectory != " " && newDirectory != string.Empty)
                {
                    var files = System.Array.Empty<string>();

                    if (newDirectory[newDirectory.Length - 1] != Path.DirectorySeparatorChar)
                        newDirectory += Path.DirectorySeparatorChar;
                    if (!Directory.Exists(newDirectory))
                        Internal.Error($"The directory \"{newDirectory}\" does not exist!");
                    files = Directory.GetFileSystemEntries(directory);

                    foreach (string subElement in files)
                    {
                        if (Directory.Exists(subElement))
                            CopyDir(subElement, newDirectory + Path.GetFileName(subElement));
                        else if (!File.Exists(newDirectory + Path.GetFileName(subElement)))
                        {
                            try
                            {
                                File.Copy(subElement, newDirectory + Path.GetFileName(subElement));
                            }
                            catch (IOException e) { Internal.Error($"AN ERROR OCCURRED:{e.Message}"); }
                        }
                    }
                }
                else Internal.Error("Empty newDirectory argument!");
            }
            else Internal.Error("Empty directory argument!");
        }

        #endregion

        /// <summary>
        /// Initial constructor for the <see cref="Copy"/> class.
        /// </summary>
        /// <param name="path">The boot path of the program.</param>
        /// <param name="arguments">Arguments given to the current command.</param>
        public Copy(string path, string[] arguments)
        {
            this.path = path;

            var file = string.Empty;
            var directory = string.Empty;
            var anotherDirectory = string.Empty;

            if (arguments.Length == 0) Internal.Error("No arguments given to the command!");
            //file
            if (arguments.Length == 1 && arguments[0] != "-h") CopySingleFile(arguments[0]);
            else if (arguments.Length == 1 && arguments[0] == "-h") new Help("copy"); 
            
            //file in directory
            if (arguments.Length == 3)
            {

                if (arguments[1] == "in" && arguments[0] != "-d")
                {
                    if (arguments[2] != string.Empty && arguments[2] != " ")
                    {
                        file = arguments[0];
                        directory = arguments[2];
                        
                        CopySingleFile(file, directory);

                    }
                    else Internal.Error("Empty directory argument!");
                }
                else Internal.Error($"Expected keyword \"IN\" in the place of \"{arguments[1]}\"!");
            }
            
            //-d directory
            else if (arguments.Length == 2)
            {
                if (arguments[0] == "-d" && arguments[1] != "in")
                {
                    directory = arguments[1];

                    if (arguments[1] != " " && arguments[1] != string.Empty)
                    {
                        if (Directory.Exists(directory))
                            CopyDir(directory);
                        else Internal.Error($"Directory \"{directory}\" does not exist!");
                    }
                    else Internal.Error("Empty directory argument!");
                }
            }
            
            //-d directory in newDirectory
            else if (arguments.Length == 4)
            {
                if ((arguments[1] != " " && arguments[3] != " ") && (arguments[1] != string.Empty && arguments[3] != string.Empty))
                {
                    directory = arguments[1];
                    anotherDirectory = arguments[3];

                    if (Directory.Exists(directory))
                        if (Directory.Exists(anotherDirectory))
                            CopyDir(directory, anotherDirectory);
                        else Internal.Error($"Directory {anotherDirectory} does not exist!");
                    else Internal.Error($"Directory {directory} does not exist!");
                }
                else Internal.Error("Empty DIRECTORY or ANOTHER_DIRECTPRY argument!");
            }

            Internal.Starter(this.path);
        }
    }
}
