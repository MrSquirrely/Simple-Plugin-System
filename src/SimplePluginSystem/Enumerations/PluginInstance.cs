namespace SimplePluginSystem
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Enumerations;
    using Library;

    /// <summary>
    /// Represents the plugin instance class.
    /// This class loads a assembly into a new domain.
    /// </summary>
    [Serializable]
    public class PluginInstance
    {
        /// <summary>
        /// Represents the app domain where the assembly will be loaded.
        /// </summary>
        private AppDomain assemblyDomain;

        /// <summary>
        /// Contains the name of the assembly.
        /// </summary>
        private string assemblyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginInstance"/> class.
        /// </summary>
        public PluginInstance()
        {
            this.assemblyDomain = null;
        }

        /// <summary>
        /// Loads a plugin (assembly) into a seperate domain.
        /// </summary>
        /// <param name="assemblyName"> Contians the name of the assembly.</param>
        /// <returns> Returns true if the loading was successful false if not.</returns>
        public bool Load(string assemblyName)
        {
            this.assemblyName = assemblyName;
            string domainName = string.Format("AssemblyDomain_{0}", this.assemblyName);

            this.assemblyDomain = AppDomain.CreateDomain(domainName);
            this.assemblyDomain.SetData("assemblyName", this.assemblyName);

            try
            {
                this.assemblyDomain.DoCallBack(() =>
                {
                    this.LoadAssembly((string)AppDomain.CurrentDomain.GetData("assemblyName"));
                });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Runs the plugin.
        /// </summary>
        public void Run()
        {
            this.assemblyDomain.SetData("assemblyName", this.assemblyName);
            this.assemblyDomain.DoCallBack(() => 
            {
                this.InvokePluginMethod(PluginActions.Initialize);
            });
        }

        /// <summary>
        /// Stops the plugin.
        /// </summary>
        public void Stop()
        {
            this.assemblyDomain.SetData("assemblyName", this.assemblyName);
            this.assemblyDomain.DoCallBack(() =>
            {
                this.InvokePluginMethod(PluginActions.Terminate);
            });
        }

        /// <summary>
        /// Unloads the domain of the assembly.
        /// </summary>
        /// <returns>Returns true if the unload process was successful or not.</returns>
        public bool Unload()
        {
            try
            {
                AppDomain.Unload(this.assemblyDomain);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Loads an assembly by its name.
        /// </summary>
        /// <param name="name"> Contains the name of the assembly to load.</param>
        private void LoadAssembly(string name)
        {
            try
            {
                Assembly assembly = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, string.Format("{0}.dll", name)));
                this.LoadDependencies(assembly.GetReferencedAssemblies());
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Loads the dependencies of the assembly.
        /// </summary>
        /// <param name="dependencies"> Contains the list of dependencies.</param>
        private void LoadDependencies(AssemblyName[] dependencies)
        {
            if (dependencies.Length == 0)
            {
                return;
            }

            foreach (AssemblyName name in dependencies)
            {
                this.LoadAssembly(name.Name);
            }
        }

        /// <summary>
        /// Performs an action with the plugin.
        /// </summary>
        /// <param name="action"> Contains the action to perform.</param>
        private void InvokePluginMethod(PluginActions action)
        {
            AppDomain domain = AppDomain.CurrentDomain;
            Assembly assembly = null;
            Assembly[] loadedAssembly = AppDomain.CurrentDomain.GetAssemblies();

            for (int i = 0; i < loadedAssembly.Length; i++)
            {
                if (loadedAssembly[i].FullName.Contains((string)domain.GetData("assemblyName")))
                {
                    assembly = loadedAssembly[i];
                    break;
                }
            }

            if (assembly == null)
            {
                return;
            }

            Type interfaceType = typeof(IPlugin);

            foreach (Type type in assembly.GetTypes().Where(p => p.IsClass && interfaceType.IsAssignableFrom(interfaceType) && p.IsPublic))
            {
                IPlugin instance = (IPlugin)Activator.CreateInstance(type, new object[] { });

                switch(action)
                {
                    case PluginActions.Initialize:
                        instance.Init();
                        break;
                    default:
                        instance.Terminate();
                        break;
                }
            }
        }
    }
}
