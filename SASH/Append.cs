using SASH.Hidden;

namespace SASH
{
    /// <summary>
    /// This struct works in conjunction with The "XMLParser" project in order to automatically create a new, custom class via a simple schema.
    /// To find the XMLParser project visit: http://www.github.com/Petaaar/XMLParser.
    /// </summary>
    struct Append
    {
        private readonly string path;

        public Append(string path, string name)
        {
            this.path = path;

            Process(name);

            Internal.Starter(path);
        }

        /// <summary>
        /// Processes the given file and attends to parse it as .sashs file.
        /// </summary>
        /// <param name="name"></param>
        private void Process(string name)
        {
            if (!System.IO.File.Exists(System.IO.Path.Combine(this.path, name)))
            {
                Internal.Error($"The given file \"{System.IO.Path.Combine(this.path, name)}\" doesn't exist!");
                Internal.Starter(this.path);
            }
            else
            {
                var fullName = System.IO.Path.Combine(this.path, name);

                if (System.IO.Path.GetExtension(fullName) != ".sashs")
                {
                    Internal.Error("Expected a \".sashs\" file!");
                    Internal.Starter(this.path);
                }
                else
                {
                    System.Console.WriteLine("Parsing...");
                    XMLParser.XMLParser parser = new XMLParser.XMLParser(fullName, true);
                    System.Console.WriteLine("DONE!");
                }
            }
        }
    }
}
