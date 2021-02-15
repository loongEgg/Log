using System.Diagnostics;

namespace LoongEgg.Log
{
    /// <summary>
    /// 调试器版的<see cref="Logger"/>
    /// </summary>
    public class DebugLogger : Logger
    {
        internal DebugLogger(Levels level) : base(level) { }

        protected override void WriteLine(string msg, Levels level = Levels.Dbug) => Debug.WriteLine(msg);
    }
}