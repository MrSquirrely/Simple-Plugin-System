namespace SimplePluginSystem.Enumerations
{
    /// <summary>
    /// Represents the types of plugin actions which can be performed by the system.
    /// </summary>
    public enum PluginActions : int
    {
        /// <summary>
        /// Initializes the plugin.
        /// </summary>
        Initialize = 0,

        /// <summary>
        /// Terminates the plugin.
        /// </summary>
        Terminate = 1
    }
}
