using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoongEgg.Log.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Enable(Loggers.Console | Loggers.Debug | Loggers.File);

            Logger.Dbug("debug");
            Logger.Info("information");
            Logger.Warn("warning");
            Logger.Erro("error");

            Logger.Clear();

            Console.ReadKey();
        }
    }
}
