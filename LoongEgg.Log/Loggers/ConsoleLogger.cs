using System;

namespace LoongEgg.Log
{
    /// <summary>
    /// 控制台版的<see cref="Logger"/>
    /// </summary>
    public class ConsoleLogger : Logger
    {
        internal ConsoleLogger(Levels level) : base(level) { }

        protected override void WriteLine(string msg, Levels level)
        {
            if (level < Level)
                return;

            ConsoleColor old = Console.ForegroundColor;
            switch (level)
            {
                case Levels.Dbug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;

                case Levels.Info:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;

                case Levels.Warn:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;

                case Levels.Erro:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                default:
                    break;
            }
            Console.WriteLine(msg);
            Console.ForegroundColor = old;
        }
    }
}
