using SASH.Hidden;

namespace SASH.OS
{
    /// <summary>
    /// Takes care of the work with environment variables.
    /// </summary>
    static class Environments
    {
        /// <summary>
        /// Shows the value of an environment variable, corresponding to the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The environment variable's name.</param>
        internal static void GetEnvironmentVariableValue(string name)
        {
            try
            {
                if (System.String.IsNullOrEmpty(name))
                {
                    Internal.Error("Empty or nullified environment variable name!");
                    return;
                }
            }
            catch (System.NullReferenceException) {  }
            System.Console.Write($"Requested environment variable with name \"{name}\". \nSystem output result:");

            string outp = System.Environment.GetEnvironmentVariable(name);

            if (System.String.IsNullOrEmpty(outp))
            {
                Internal.Error("\nNo environment variable with the requested name exists!");
                return;
            }
            if (outp.Contains(";"))
            {
                System.Console.WriteLine(""); //break the line from above...
                string[] outpArr = outp.Split(new char[] { ';' }, System.StringSplitOptions.None);

                foreach (string item in outpArr) System.Console.WriteLine(item);
            }
            else System.Console.WriteLine(outp);
        }
    }
}
