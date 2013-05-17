using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSCT.Helpers;

namespace WSCT.IronPython.Execute
{
    class Program
    {
        static void Main(string[] args)
        {
            String pythonFileName;
            if (args.Length == 0)
                pythonFileName = @"wsct_entry.py";
            else
                pythonFileName = args[0];

            String xmlFileName;
            if (args.Length == 0 || args.Length == 1)
                xmlFileName = @"wsct_entry.xml";
            else
                xmlFileName = args[1];

            IronPythonRuntime iPyRuntime = new IronPythonRuntime(pythonFileName);

            iPyRuntime.addAssemblies(SerializedObject<AssemblyRepository>.loadFromXml(xmlFileName));

            iPyRuntime.execute();

            dynamic demoObject = iPyRuntime.execute().scope.wsct_entry();
        }
    }
}
