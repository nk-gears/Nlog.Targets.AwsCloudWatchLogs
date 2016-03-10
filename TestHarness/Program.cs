using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace TestHarness
{
    class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Logger.Trace("Sample trace message");
            Logger.Debug("Sample debug message");
            Logger.Info("Sample informational message");
            Logger.Warn("Sample warning message");
            Logger.Error("Sample error message");
            Logger.Fatal("Sample fatal error message");

            Thread.Sleep(4000);

            Logger.Trace("Sample trace message");
            Logger.Debug("Sample debug message");
            Logger.Info("Sample informational message");
            Logger.Warn("Sample warning message");
            Logger.Error("Sample error message");
            Logger.Fatal("Sample fatal error message");

            Thread.Sleep(4000);

            Logger.Trace("Sample trace message");
            Logger.Debug("Sample debug message");
            Logger.Info("Sample informational message");
            Logger.Warn("Sample warning message");
            Logger.Error("Sample error message");
            Logger.Fatal("Sample fatal error message");


            Console.ReadLine();
        }
    }
}
