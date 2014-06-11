using System;
using System.Collections.Generic;
using System.Reflection;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace WSCT.IronPython
{
    public class IronPythonRuntime
    {
        private readonly List<String> assemblyFileNames;
        private readonly String ipySource;
        private readonly ScriptEngine pythonEngine;

        /// <summary>
        /// <see cref="ScriptScope"/> object used to access functions, data and classes in Python context.
        /// <c>scope</c> is instantiated at object creation time to be used to pass variables to runtime in <see cref="Execute"/> method.
        /// </summary>
        public dynamic Scope;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ipySourceFileName">Source file name</param>
        public IronPythonRuntime(String ipySourceFileName)
        {
            ipySource = ipySourceFileName;
            assemblyFileNames = new List<string>();

            pythonEngine = Python.CreateEngine();
            Scope = pythonEngine.CreateScope();
        }

        /// <summary>
        /// Declares an assembly to be added into Python Runtime.
        /// </summary>
        /// <param name="assemblyFileName">File name of an assembly to execute into Python Runtime.</param>
        /// <returns>The current object.</returns>
        public IronPythonRuntime AddAssembly(String assemblyFileName)
        {
            assemblyFileNames.Add(assemblyFileName);
            return this;
        }

        /// <summary>
        /// Declare assemblies contained in a repository to be added into Python Runtime.
        /// </summary>
        /// <param name="assemblyRepository">Repository of assemblies to execute into Python Runtime.</param>
        /// <returns>The current object.</returns>
        public IronPythonRuntime AddAssemblies(AssemblyRepository assemblyRepository)
        {
            foreach (var description in assemblyRepository.assemblies)
            {
                AddAssembly(description.pathToDll + description.dllName);
            }
            return this;
        }

        /// <summary>
        /// Load and execute the python script.
        /// </summary>
        /// <returns>The current object.</returns>
        public IronPythonRuntime Execute()
        {
            foreach (var assemblyFileName in assemblyFileNames)
            {
                pythonEngine.Runtime.LoadAssembly(Assembly.LoadFrom(assemblyFileName));
            }

            pythonEngine.ExecuteFile(ipySource, Scope);

            return this;
        }
    }
}