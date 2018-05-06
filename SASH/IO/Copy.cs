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
                    catch (IOException e) { Internal.Error($"{e.Message}"); }
                }
                else Internal.Error("Empty path argument!");
            }
            else Internal.Error("Empty file argument!");

            Internal.Starter(this.path);
        }

        /// <summary>
        /// Copies a <paramref name="directory"/> in the boot one.
        /// </summary>
        /// <param name="directory">Directory to copy.</param>
        private void CopyDir(string directory)
        {

        }

        /// <summary>
        /// Copies a <paramref name="directory"/> 
        /// in another specified <paramref name="newDirectory"/>.
        /// </summary>
        /// <param name="directory">Directory to be copied.</param>
        /// <param name="newDirectory">Directory to copy the <paramref name="directory"/> to.</param>
        private void CopyDir(string directory, string newDirectory)
        {

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
            if (arguments.Length == 1) CopySingleFile(arguments[0]);

            if (arguments.Length == 2) Internal.Error("Too many or too much arguments given!");
            
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
                    if (Directory.Exists(directory))
                        if (Directory.Exists(anotherDirectory))
                            if (Directory.Exists(Path.Combine(anotherDirectory, directory)))
                                Internal.Error($"{directory} exists in {anotherDirectory}!");
                            else CopyDir(directory, anotherDirectory);
                        else Internal.Error($"Directory {anotherDirectory} does not exist!");
                    else Internal.Error($"Directory {directory} does not exist!");
                }
                else Internal.Error("Empty DIRECTORY or ANOTHER_DIRECTPRY argument!");
            }

            Internal.Starter(this.path);
        }
    }
}
