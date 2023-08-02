using System;
using System.IO;
using CLAP;

namespace Quoridor.ConsoleApp.Utils
{
    public class BaseRunner
    {
        private readonly TextWriter _stdOut;
        private readonly TextWriter _stdErr;

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
