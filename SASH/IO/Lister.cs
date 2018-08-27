using SASH.Hidden;
using System.IO;

namespace SASH.IO
{
    /// <summary>
    /// Lists an entire given directory with specified arguments.
    /// </summary>
    class Lister
    {
        private readonly string path;

        private readonly string arg;

        private readonly bool noArgs;

        private System.Collections.Specialized.StringCollection items;

        private void SaveFolders()
        {
            foreach (string folder in Directory.GetDirectories(this.path))
                if (folder.Length > 30)
                    items.Add($"[F]{Path.GetDirectoryName(folder)}");
                else
                    items.Add($"{folder}");
        }

        private void SaveFiles()
        {
            foreach (var file in Directory.GetFiles(this.path))
                items.Add(Path.GetFileName(file));
        }

        private void Traverse(ArgumentType argument)
        {
            if (argument == ArgumentType.EMPTY)
            {
                SaveFiles();
                SaveFolders();
            }
            else if (argument == ArgumentType.FOLDERS)
                SaveFolders();
            else if (argument == ArgumentType.FILES)
                SaveFiles();

            else
            {
                Internal.Error("Invalid argument!");
                Internal.Starter(this.path);
            }
        }

        private void CheckArgs()
        {
            this.items = new System.Collections.Specialized.StringCollection(); 

            if (noArgs || arg == "all")
                Traverse(ArgumentType.EMPTY);
            else
            {
                if (arg == "files" || arg == "-f")
                    Traverse(ArgumentType.FILES);
                if (arg == "folders" || arg == "-fl")
                    Traverse(ArgumentType.FOLDERS);
            }
        }

        public Lister(string path, string arg)
        {
            if (arg.Length == 0)
                this.noArgs = true;
            else
                this.arg = arg;
            this.path = path;

            CheckArgs();
            char items = this.items.Count == 1 ? ' ' : 's';
            
            if (this.items.Count == 0)
            {
                System.Console.WriteLine("No items found in the current directory to display.");
                Internal.Starter(this.path);
            }

            Internal.Ask($"{this.items.Count} item{items} found. Display everything?[y/n]:");
            char yn = System.Console.ReadLine().ToCharArray()[0];
            try
            {
                if (this.items.Count == 0)
                    Internal.Starter(this.path);
                if (yn == 'y' && this.items.Count != 0 && this.items != null)
                    foreach (string item in this.items)
                        System.Console.WriteLine(item);
                Internal.Starter(this.path);
            }
            catch (System.ArgumentOutOfRangeException) { Internal.Starter(this.path); }
            catch (System.IndexOutOfRangeException) { Internal.Starter(this.path); }
            catch (System.NullReferenceException) { Internal.Error("No items to display!"); Internal.Starter(this.path); }

        }
    }

    enum ArgumentType
    {
        EMPTY = 0,
        FILES = 1,
        FOLDERS = 2
    }
}
