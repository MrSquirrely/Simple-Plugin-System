namespace SimplePluginSystem.Library
{
    /// <summary>
    /// Represents the plugin interface.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Represents the initialize method.
        /// This method will be called when the plugin will be invoked.
        /// </summary>
        void Init();

        /// <summary>
        /// Represents the terminate method.
        /// This method will be called when the plugin will be terminated.
        /// </summary>
        void Terminate();
    }
}
