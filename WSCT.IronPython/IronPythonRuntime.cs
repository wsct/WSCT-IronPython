using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace WSCT.IronPython
{
    public class IronPythonRuntime
    {
        String ipySource;
        List<String> assemblyFileNames;
        ScriptEngine pythonEngine;

        /// <summary>
        /// <see cref="ScriptScope"/> object used to access functions, data and classes in Python context.
        /// <c>scope</c> is instantiated at object creation time to be used to pass variables to runtime in <see cref="execute"/> method
        /// </summary>
        public dynamic scope;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ipySourceFileName">Source file name</param>
        public IronPythonRuntime(String ipySourceFileName)
        {
            this.ipySource = ipySourceFileName;
            this.assemblyFileNames = new List<string>();

            this.pythonEngine = Python.CreateEngine();
            this.scope = pythonEngine.CreateScope();
        }

        /// <summary>
        /// Declare an assembly to be added into Python Runtime
        /// </summary>
        /// <param name="assemblyFileName">File name of an assembly to execute into Python Runtime</param>
        /// <returns>The current object</returns>
        public IronPythonRuntime addAssembly(String assemblyFileName)
        {
            assemblyFileNames.Add(assemblyFileName);
            return this;
        }

        /// <summary>
        /// Declare assemblies contained in a repository to be added into Python Runtime
        /// </summary>
        /// <param name="assemblyRepository">Repository of assemblies to execute into Python Runtime</param>
        /// <returns>The current object</returns>
        public IronPythonRuntime addAssemblies(AssemblyRepository assemblyRepository)
        {
            foreach (AssemblyDescription description in assemblyRepository.assemblies)
                addAssembly(description.pathToDll + description.dllName);
            return this;
        }

        /// <summary>
        /// Load and execute the python script.
        /// </summary>
        /// <returns>The current object</returns>
        public IronPythonRuntime execute()
        {
            foreach (String assemblyFileName in assemblyFileNames)
                pythonEngine.Runtime.LoadAssembly(Assembly.LoadFrom(assemblyFileName));

            pythonEngine.ExecuteFile(ipySource, scope);

            return this;
        }
    }
}
