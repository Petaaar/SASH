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

        /// <summary>
        /// Creates an environment variable by given <paramref name="name"/>, <paramref name="value"/> and optionally - <paramref name="target"/>.
        /// </summary>
        /// <param name="name">The name of the environment variable.</param>
        /// <param name="value">The value of the environment variable.</param>
        /// <param name="target">The <see cref="System.EnvironmentVariableTarget"/> of the variable.</param>
        internal static void CreateEnvironmentVariable(string name, string value, System.EnvironmentVariableTarget target = System.EnvironmentVariableTarget.User)
        {
            try
            {
                System.Environment.SetEnvironmentVariable(name, value, target);
            }
            catch(System.Exception)
            {
                Internal.Error($"Cannot create environment variable with name {name} and value {value} due to exception.");
            }
        }
    }

    
}
