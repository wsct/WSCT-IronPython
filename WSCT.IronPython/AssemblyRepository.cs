using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Linq;

namespace WSCT.IronPython
{
    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("Assemblies")]
    public class AssemblyRepository
    {
        #region >> Fields

        List<AssemblyDescription> _assemblies;

        #endregion

        #region >> Properties

        /// <summary>
        /// Assemblies Count
        /// </summary>
        [XmlIgnore]
        public int count
        {
            get
            {
                return _assemblies.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("assembly")]
        public List<AssemblyDescription> assemblies
        {
            get { return _assemblies; }
            set { _assemblies = value; }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AssemblyRepository()
        {
            _assemblies = new List<AssemblyDescription>();
        }

        #endregion

        #region >> Methods

        /// <summary>
        /// Add a new assemblyDesc descriptor to the manager
        /// </summary>
        /// <param name="assemblyDesc">Descriptor of the assemblyDesc to be added</param>
        public void add(AssemblyDescription assemblyDesc)
        {
            _assemblies.Add(assemblyDesc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyName">Name of the assemblyDesc to look for</param>
        /// <returns><c>true</c> if the AssemblyDescription exists</returns>
        public Boolean isValid(String assemblyName)
        {
            return _assemblies.Where(a => a.name == assemblyName).FirstOrDefault() != null;
        }

        /// <summary>
        /// Get the <c>AssemblyDescription</c> instance which name is <c>assemblyName</c>
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns>The AssemblyDescription instance or null if not find</returns>
        public AssemblyDescription get(String assemblyName)
        {
            AssemblyDescription assemblyFound = null;
            foreach (AssemblyDescription assembly in _assemblies)
                if (assembly.name == assemblyName && assembly.isValid)
                {
                    assemblyFound = assembly;
                    break;
                }
            return assemblyFound;
        }

        #endregion
    }
}
