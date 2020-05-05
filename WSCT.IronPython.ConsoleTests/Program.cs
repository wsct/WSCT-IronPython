using System;
using System.Reflection;
using IronPython.Hosting;
using WSCT.Core;
using WSCT.Helpers;
using WSCT.Wrapper.Desktop.Core;

namespace WSCT.IronPython.ConsoleTests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // simpleExample(args);
            // wsctHelperExample(args);
            WsctCoreExample(args);

            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }

        private static void WsctCoreExample(string[] args)
        {
            var foregroundColor = Console.ForegroundColor;

            Console.WriteLine("C#  >> Initializing iPy runtime environment");

            var ironPythonRuntime = new IronPythonRuntime("wsctcore.py");
            ironPythonRuntime
                .AddAssembly("WSCT.Helpers.dll")
                .AddAssembly("WSCT.dll")
                .AddAssembly("WSCT.Wrapper.Desktop.dll")
                .Execute();
            var demoObject = ironPythonRuntime.Scope.WSCTCoreDemo();

            Console.WriteLine("C#  >> Asking iPy to establish PC/SC connexion");
            demoObject.initializeContext();
            ICardContext context = demoObject.context;
            Console.WriteLine("C#  >> context.isValid(): {0}", context.IsValid());

            Console.WriteLine("C#  >> Waiting for a card to be inserted");
            var monitor = new StatusChangeMonitor(context);
            var readerState = monitor.WaitForCardPresence(0);
            if (readerState == null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("C#  >> Insert a card in one of the {0} readers (time out in 15s)", context.ReadersCount);
                Console.ForegroundColor = foregroundColor;
                readerState = monitor.WaitForCardPresence(15000);
            }

            if (readerState == null)
            {
                Console.WriteLine("C#  >> Time Out! No card found");
            }
            else
            {
                demoObject.useReader(readerState.ReaderName);
            }

            Console.WriteLine("C#  >> Asking iPy to close PC/SC connection");
            demoObject.terminateContext();
        }

        private static void WsctHelperExample(string[] args)
        {
            var pythonEngine = Python.CreateEngine();
            pythonEngine.Runtime.LoadAssembly(Assembly.GetAssembly(typeof(BytesHelpers)));
            dynamic demoScript = pythonEngine.Runtime.UseFile("wscthelper.py");

            var demoObject = demoScript.ArrayOfBytesDemo("1PAY.SYS.DDF01");
            demoObject.doIt();
        }

        private static void SimpleExample(string[] args)
        {
            // Use of ScriptRuntime
            var pythonRuntime = Python.CreateRuntime();

            // Use of dynamic keyword to access python functions and classes
            dynamic dynamicSource = pythonRuntime.UseFile("simple.py");
            dynamicSource.Simple();
            var dynamicMyClass = dynamicSource.MyClass();
            dynamicMyClass.somemethod();

            // Use of ScriptEngine
            var pythonEngine = Python.CreateEngine();
            // Definition of scope
            var scope = pythonEngine.CreateScope();
            scope.SetVariable("test", "test me");

            var source = pythonEngine.CreateScriptSourceFromFile("simple.py");
            var compiled = source.Compile();
            compiled.Execute(scope);

            // Get the Python Class
            object myClass = pythonEngine.Operations.Invoke(scope.GetVariable("MyClass"));
            // Invoke a method of the class
            pythonEngine.Operations.InvokeMember(myClass, "somemethod", new object[0]);

            // create a callable function to 'somemethod'
            var someMethod2 = pythonEngine.Operations.GetMember<Action>(myClass, "somemethod");
            someMethod2();

            // create a callable function to 'isodd'
            var isOdd = pythonEngine.Operations.GetMember<Func<int, bool>>(myClass, "isodd");
            Console.WriteLine(isOdd(1).ToString());
            Console.WriteLine(isOdd(2).ToString());

            var callDir = pythonEngine.Operations.GetMember<Action>(myClass, "calldir");
            callDir();

            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }
    }
}
