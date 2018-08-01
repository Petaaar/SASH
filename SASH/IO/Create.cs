using System;
using System.IO;
using SASH.Hidden;

namespace SASH.IO
{
    class Create
    {
        private string path;

        /// <summary>
        /// Creates a file / folder in the given in the <paramref name="arguments"/> directory.
        /// If there is not given any directory argument, the file is created in the current one.
        /// </summary>
        /// <param name="arguments">The arguments to the command.</param>
        public Create(string path, string[] arguments)
        {
            this.path = path;

            string filename = string.Empty;
            string destination = string.Empty;
            string[] filesArr = Array.Empty<string>();
            int argsLen = arguments.Length;

            if (argsLen == 1 && arguments[0] != "-h") filename = arguments[0];
            else if (argsLen == 1 && arguments[0] == "-h") new Help("create");

            if (argsLen == 3 && arguments[1] == "in")
            {
                filename = arguments[0];
                if (Directory.Exists(arguments[2]) || arguments[2] == nameof(path))
                {
                    string argsLast = arguments[2];

                    if (argsLast[argsLast.Length - 1] == @"\".ToCharArray()[0])
                        destination = argsLast;
                    else
                        destination = argsLast + @"\";
                }
                else
                {
                    Internal.Error("Destination does not exist!");
                    //TODO: CREATING A NEW DESTINATION!!!
                    Internal.Starter(this.path);
                }
            }

            if (destination == nameof(path) || argsLen == 1)
                destination = this.path;

            if (IsListOfFiles(filename))
            {
                filesArr = filename.Split(new char[] { ',', '{', '}' }, StringSplitOptions.None);

                var files = filesArr.ToList();

                //fixing the "path" issue.
                files.Remove(files[0]);
                files.Remove(files[files.Count - 1]);

                System.Threading.Tasks.Parallel.For(0, files.Count,
                    x => CreateFile(destination + files[x]));

                Internal.Starter(this.path);
            }
            else
            {
                if (File.Exists(destination + filename))
                {
                    Internal.Error("File already exists in the specified destination!");
                    Internal.Starter(this.path); //save from cross-thread operation!
                }
                else
                {
                    CreateFile(destination + filename);
                    Internal.Starter(this.path);
                }
            }

            if (argsLen == 1) CreateFile(destination + filename);

            Internal.Starter(this.path);

        }
    #pragma warning restore


        /// <summary>
        /// Creates a file with given <paramref name="filename"/>
        /// </summary>
        /// <param name="filename">The name of the file to be created.</param>
        private void CreateFile(string filename)
        {
            try
            {
                File.Create(filename);
                Console.WriteLine($"Created a new file in {filename}!");
                Internal.Starter(this.path);
            }
            catch (IOException) {; }
        }

        /// <summary>
        /// Determines whenever a string is a list of arguments needed for
        /// the <see cref="Create(string[])"/> function.
        /// </summary>
        /// <param name="argument"></param>
        /// <returns>boolean</returns>
        private static bool IsListOfFiles(string argument)
            => argument[0] == '{' & argument[argument.Length - 1] == '}';

    }
}