using CLAP;
using System;
using System.IO;

namespace Quoridor.ConsoleApp.Utils
{
    public class BaseRunner
    {
        public readonly TextWriter _stdOut;
        public readonly TextWriter _stdErr;

        public BaseRunner(TextWriter stdOut, TextWriter stdErr)
        {
            _stdOut = stdOut;
            _stdErr = stdErr;
        }

        [Help]
        public void HelpMessage(string message)
        {
            _stdOut.WriteLine(message);
            Console.ReadKey();
        }

        [Error]
        public void HandleError(ExceptionContext context)
        {
            _stdErr.WriteLine(context.Exception.Message);
            Console.ReadKey();
        }

    }
}
