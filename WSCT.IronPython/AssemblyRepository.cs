using System.Collections.Generic;
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

        #endregion

        #region >> Properties

        /// <summary>
        /// Assemblies Count
        /// </summary>
        [XmlIgnore]
        public int Count => Assemblies.Count;

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("assembly")]
        public List<AssemblyDescription> Assemblies { get; set; }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AssemblyRepository()
        {
            Assemblies = new List<AssemblyDescription>();
        }

        #endregion

        #region >> Methods

        /// <summary>
        /// Add a new assemblyDesc descriptor to the manager
        /// </summary>
        /// <param name="assemblyDesc">Descriptor of the assemblyDesc to be added</param>
        public void Add(AssemblyDescription assemblyDesc)
        {
            Assemblies.Add(assemblyDesc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyName">Name of the assemblyDesc to look for</param>
        /// <returns><c>true</c> if the AssemblyDescription exists</returns>
        public bool IsValid(string assemblyName)
        {
            return Assemblies.FirstOrDefault(a => a.Name == assemblyName) != null;
        }

        /// <summary>
        /// Get the <c>AssemblyDescription</c> instance which name is <c>assemblyName</c>
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns>The AssemblyDescription instance or null if not find</returns>
        public AssemblyDescription Get(string assemblyName)
        {
            return Assemblies.FirstOrDefault(assembly => assembly.Name == assemblyName && assembly.IsValid);
        }

        #endregion
    }
}
