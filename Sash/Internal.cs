/// <summary>
/// A static class with custom program (or extension) methods.
/// </summary>
internal static class Internal
{
    /// <summary>
    /// Kills the CMD and the program.
    /// </summary>
    public static void KillCmd()
        => System.Array.ForEach(
            System.Diagnostics.Process.GetProcessesByName("cmd"),
                    x => x.Kill());

    /// <summary>
    /// Writes a custom-colored "error" <paramref name="message"/>.
    /// </summary>
    /// <param name="message">The error message to be written.</param>
    public static void Error(string message)
    {
        System.Console.ForegroundColor = System.ConsoleColor.Red;
        System.Console.WriteLine(message);
        System.Console.ForegroundColor = System.ConsoleColor.Gray;
    }

    /// <summary>
    /// A method to make the program "sleep" for specified <paramref name="millis"/>
    /// </summary>
    /// <param name="millis">Milliseconds specified to make the program sleep.</param>
    public static void Sleep(int millis)
        => System.Threading.Thread.Sleep(millis);

    /// <summary>
    /// Creates a new object instance of the <see cref="SASH.Starter"/> class.
    /// </summary>
    /// <param name="path">The starting path, where the program will operate.</param>
    /// <returns>new Starter() object.</returns>
    public static SASH.Starter Starter(string path) 
        => new SASH.Starter(path);
}
