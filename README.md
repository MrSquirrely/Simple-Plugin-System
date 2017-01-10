# Simple-Plugin-System
A simple plugin system in c#

## Main classes / Interfaces

1. PluginInstance
  * Load a dll (Plugin).
  * Invoke the plugin.
  * Terminate the plugin.
  * Unload the dll (Plugin).

2. IPlugin
  * The main guidline for a plugin.

## Available namespaces

1. SimplePluginSystem
2. SimplePluginSystem.Library

## Example

To learn how to use this library in the correct way, please have a look at this short example.

## The extendable Application 

```cs 
        /// <summary>
        /// Contains the main method of our program.
        /// </summary>
        /// <param name="args"> Contains the command line arguments.</param>
        public static void Main(string[] args)
        {
            PluginInstance plugin = new PluginInstance();
            plugin.Load("TestLib");
            plugin.Run();
            plugin.Stop();
        }
```

## Plugin as dll Library

```cs 
    /// <summary>
    /// Represents the main class of our plugin.
    /// </summary>
    public class MyCoolPlugin : IPlugin
    {
        /// <summary>
        /// Represents the initialize method.
        /// This method will be called when the plugin will be invoked.
        /// </summary>
        public void Init()
        {
            Console.WriteLine("Hello from the plugin");
        }

        /// <summary>
        /// Represents the terminate method.
        /// This method will be called when the plugin will be terminated.
        /// </summary>
        public void Terminate()
        {
            Console.WriteLine("ByeBye from the plugin");
        }
    }
```
