using System.Collections.Generic;
using System.IO;
using System.Reflection;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace WSCT.IronPython
{
    public class IronPythonRuntime
    {
        private readonly List<string> _assemblyFileNames;
        private readonly string _ipySource;
        private readonly ScriptEngine _pythonEngine;

        /// <summary>
        /// <see cref="ScriptScope"/> object used to access functions, data and classes in Python context.
        /// <c>scope</c> is instantiated at object creation time to be used to pass variables to runtime in <see cref="Execute"/> method.
        /// </summary>
        public dynamic Scope;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ipySourceFileName">Source file name</param>
        public IronPythonRuntime(string ipySourceFileName)
        {
            _ipySource = ipySourceFileName;
            _assemblyFileNames = new List<string>();

            _pythonEngine = Python.CreateEngine();
            Scope = _pythonEngine.CreateScope();
        }

        /// <summary>
        /// Declares an assembly to be added into Python Runtime.
        /// </summary>
        /// <param name="assemblyFileName">File name of an assembly to execute into Python Runtime.</param>
        /// <returns>The current object.</returns>
        public IronPythonRuntime AddAssembly(string assemblyFileName)
        {
            _assemblyFileNames.Add(assemblyFileName);

            return this;
        }

        /// <summary>
        /// Declare assemblies contained in a repository to be added into Python Runtime.
        /// </summary>
        /// <param name="assemblyRepository">Repository of assemblies to execute into Python Runtime.</param>
        /// <returns>The current object.</returns>
        public IronPythonRuntime AddAssemblies(AssemblyRepository assemblyRepository)
        {
            foreach (var description in assemblyRepository.Assemblies)
            {
                AddAssembly(Path.Combine(description.PathToDll, description.DllName));
            }
            return this;
        }

        /// <summary>
        /// Load and execute the python script.
        /// </summary>
        /// <returns>The current object.</returns>
        public IronPythonRuntime Execute()
        {
            foreach (var assemblyFileName in _assemblyFileNames)
            {
                _pythonEngine.Runtime.LoadAssembly(Assembly.LoadFrom(assemblyFileName));
            }

            _pythonEngine.ExecuteFile(_ipySource, Scope);

            return this;
        }
    }
}