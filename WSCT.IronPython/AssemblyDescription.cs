using System.Xml.Serialization;

namespace WSCT.IronPython
{
    /// <summary>
    /// 
    /// </summary>
    public class AssemblyDescription
    {
        #region >> Fields

        private string _name;
        private string _dllName;
        private string _pathToDll;

        #endregion

        #region >> Constructors

        /// <summary>
        /// 
        /// </summary>
        public AssemblyDescription()
        {
            _pathToDll = "";
        }

        #endregion

        #region >> Properties

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get => _name;
            set => _name = value == "" ? null : value;
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("dll")]
        public string DllName
        {
            get => _dllName;
            set => _dllName = value == "" ? null : value;
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("pathToDll")]
        public string PathToDll
        {
            get => _pathToDll;
            set => _pathToDll = value ?? "";
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public bool IsValid => (Name != null && _dllName != null);

        #endregion
    }
}
