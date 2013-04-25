using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using WSCT.Core;
using WSCT.Wrapper;
using WSCT.IronPython;
using IronPython;

namespace WSCT.IronPython
{
    class Program
    {
        static void Main(string[] args)
        {
            // simpleExample(args);
            // wsctHelperExample(args);
            wsctCoreExample(args);

            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }

        static void wsctCoreExample(string[] args)
        {
            ConsoleColor foregroundColor = Console.ForegroundColor;

            Console.WriteLine("C#  >> Initializing iPy runtime environment");

            IronPythonRuntime helper = new IronPythonRuntime("wsctcore.py");
            helper
                .addAssembly("WSCT.Helpers.dll")
                .addAssembly("WSCT.Wrapper.dll")
                .addAssembly("WSCT.Core.dll")
                .execute();
            dynamic demoObject = helper.scope.WSCTCoreDemo();

            Console.WriteLine("C#  >> Asking iPy to establish PC/SC connexion");
            demoObject.initializeContext();
            ICardContext context = demoObject.context;
            Console.WriteLine("C#  >> context.isValid(): {0}", context.isValid());

            Console.WriteLine("C#  >> Waiting for a card to be inserted");
            StatusChangeMonitor monitor = new StatusChangeMonitor(context);
            AbstractReaderState readerState = monitor.waitForCardPresence(0);
            if (readerState == null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("C#  >> Insert a card in one of the {0} readers (time out in 15s)", context.readersCount);
                Console.ForegroundColor = foregroundColor;
                readerState = monitor.waitForCardPresence(15000);
            }

            if (readerState == null)
            {
                Console.WriteLine("C#  >> Time Out! No card found");
            }
            else
            {
                demoObject.useReader(readerState.readerName);
            }

            Console.WriteLine("C#  >> Asking iPy to close PC/SC connexion");
            demoObject.terminateContext();
        }

        static void wsctHelperExample(string[] args)
        {
            ScriptEngine pythonEngine = Python.CreateEngine();
            pythonEngine.Runtime.LoadAssembly(System.Reflection.Assembly.GetAssembly(typeof(WSCT.Helpers.ArrayOfBytes)));
            dynamic demoScript = pythonEngine.Runtime.UseFile("wscthelper.py");

            dynamic demoObject = demoScript.ArrayOfBytesDemo("1PAY.SYS.DDF01");
            demoObject.doIt();
        }

        static void simpleExample(string[] args)
        {
            // Use of ScriptRuntime
            ScriptRuntime pythonRuntime = Python.CreateRuntime();

            // Use of dynamic keyword to access python functions and classes
            dynamic dynamicSource = pythonRuntime.UseFile("simple.py");
            dynamicSource.Simple();
            dynamic dynamicMyClass = dynamicSource.MyClass();
            dynamicMyClass.somemethod();

            // Use of ScriptEngine
            ScriptEngine pythonEngine = Python.CreateEngine();
            // Definition of scope
            ScriptScope scope = pythonEngine.CreateScope();
            scope.SetVariable("test", "test me");

            ScriptSource source = pythonEngine.CreateScriptSourceFromFile("simple.py");
            CompiledCode compiled = source.Compile();
            compiled.Execute(scope);

            // Get the Python Class
            object myClass = pythonEngine.Operations.Invoke(scope.GetVariable("MyClass"));
            // Invoke a method of the class
            pythonEngine.Operations.InvokeMember(myClass, "somemethod", new object[0]);

            // create a callable function to 'somemethod'
            Action SomeMethod2 = pythonEngine.Operations.GetMember<Action>(myClass, "somemethod");
            SomeMethod2();

            // create a callable function to 'isodd'
            Func<int, bool> IsOdd = pythonEngine.Operations.GetMember<Func<int, bool>>(myClass, "isodd");
            Console.WriteLine(IsOdd(1).ToString());
            Console.WriteLine(IsOdd(2).ToString());

            Action callDir = pythonEngine.Operations.GetMember<Action>(myClass, "calldir");
            callDir();

            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }
    }
}
