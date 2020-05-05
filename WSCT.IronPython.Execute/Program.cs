using WSCT.Helpers;

namespace WSCT.IronPython.Execute
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string pythonFileName;
            if (args.Length == 0)
            {
                pythonFileName = @"wsct_entry.py";
            }
            else
            {
                pythonFileName = args[0];
            }

            string xmlFileName;
            if (args.Length == 0 || args.Length == 1)
            {
                xmlFileName = @"wsct_entry.xml";
            }
            else
            {
                xmlFileName = args[1];
            }

            var iPyRuntime = new IronPythonRuntime(pythonFileName);

            iPyRuntime.AddAssemblies(SerializedObject<AssemblyRepository>.LoadFromXml(xmlFileName));

            iPyRuntime.Execute();

            iPyRuntime.Execute().Scope.wsct_entry();
        }
    }
}