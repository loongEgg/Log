using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace LoongEgg.Log
{
    /// <summary>
    /// logger的基类, 同时也是管理器
    /// </summary>
    public abstract class Logger
    {
        /// <summary>
        /// 当前log实例的字典
        /// </summary>
        public static Dictionary<Loggers, Logger> LoggerDict { get; } = new Dictionary<Loggers, Logger>();
         
        /// <summary>
        /// <see cref="Levels"/>
        /// </summary>
        public Levels Level { get; protected set; }

        /// <summary>
        /// 默认构造器
        /// </summary>
        /// <param name="level"><see cref="Levels"/></param>
        protected Logger(Levels level)
        {
            Level = level;
        }

        /// <summary>
        /// 使能指定类型的logger
        /// </summary>
        /// <param name="loggers">使能的类型可以使用或(比如: loggers.Debug | logger.Console)</param>
        /// <param name="level"><see cref="Levels"/></param>
        public static void Enable(Loggers loggers, Levels level = Levels.Dbug)
        {
            lock (_Lock)
            {
                if (loggers.HasFlag(Loggers.Console))
                {
                    if (!LoggerDict.ContainsKey(Loggers.Console))
                    {
                        LoggerDict.Add(Loggers.Console, new ConsoleLogger(level));
                    }
                }
                if (loggers.HasFlag(Loggers.Debug))
                {
                    if (!LoggerDict.ContainsKey(Loggers.Debug))
                    {
                        LoggerDict.Add(Loggers.Debug, new DebugLogger(level));
                    }
                }
                if (loggers.HasFlag(Loggers.File))
                {
                    if (!LoggerDict.ContainsKey(Loggers.File))
                    {
                        LoggerDict.Add(Loggers.File, new FileLogger(level));
                    }
                }
                foreach (var log in LoggerDict.Values)
                {
                    log.Level = level;
                }
            }
        }

        /// <summary>
        /// 清除所有记录器
        /// </summary>
        public static void Clear() => LoggerDict.Clear();

        /// <summary>
        /// 禁止指定类型的记录器工作
        /// </summary>
        /// <param name="logger"><see cref="Loggers"/></param>
        public static void Disable(Loggers logger)
        {
            var enums = Enum.GetValues(typeof(Loggers));
            foreach (Loggers item in enums)
            {
                if (logger.HasFlag(item) && LoggerDict.ContainsKey(item))
                    LoggerDict.Remove(item);
            }
        }

        /// <summary>
        /// 打印一则<see cref="Levels.Dbug"/>调试级别的log
        /// </summary>
        /// <param name="message">消息主体</param>
        /// <param name="isDetailedMode">详细模式?</param>
        /// <param name="callerPath">[自动获取]调用文件路径</param>
        /// <param name="callerLine">[自动获取]调用行</param>
        /// <param name="callerMethod">[自动获取]调用方法名</param>
        public static void Dbug(
            string message,
            bool isDetailedMode = false,
            [CallerFilePath] string callerPath = null,
            [CallerLineNumber] int callerLine = 0,
            [CallerMemberName] string callerMethod = null) => WriteLine(message, Levels.Dbug, isDetailedMode, callerPath, callerLine, callerMethod);

        /// <summary>
        /// 打印一则<see cref="Levels.Info"/>消息级别的log
        /// </summary>
        /// <param name="message">消息主体</param>
        /// <param name="isDetailedMode">详细模式?</param>
        /// <param name="callerPath">[自动获取]调用文件路径</param>
        /// <param name="callerLine">[自动获取]调用行</param>
        /// <param name="callerMethod">[自动获取]调用方法名</param>
        public static void Info(
            string message,
            bool isDetailedMode = false,
            [CallerFilePath] string callerPath = null,
            [CallerLineNumber] int callerLine = 0,
            [CallerMemberName] string callerMethod = null) => WriteLine(message, Levels.Info, isDetailedMode, callerPath, callerLine, callerMethod);

        /// <summary>
        /// 打印一则<see cref="Levels.Warn"/>警告级别的log
        /// </summary>
        /// <param name="message">消息主体</param>
        /// <param name="isDetailedMode">详细模式?</param>
        /// <param name="callerPath">[自动获取]调用文件路径</param>
        /// <param name="callerLine">[自动获取]调用行</param>
        /// <param name="callerMethod">[自动获取]调用方法名</param>
        public static void Warn(
          string message,
          bool isDetailedMode = false,
          [CallerFilePath] string callerPath = null,
          [CallerLineNumber] int callerLine = 0,
          [CallerMemberName] string callerMethod = null) => WriteLine(message, Levels.Warn, isDetailedMode, callerPath, callerLine, callerMethod);

        /// <summary>
        /// 打印一则<see cref="Levels.Erro"/>错误级别的log
        /// </summary>
        /// <param name="message">消息主体</param>
        /// <param name="isDetailedMode">详细模式?</param>
        /// <param name="callerPath">[自动获取]调用文件路径</param>
        /// <param name="callerLine">[自动获取]调用行</param>
        /// <param name="callerMethod">[自动获取]调用方法名</param>
        public static void Erro(
          string message,
          bool isDetailedMode = false,
          [CallerFilePath] string callerPath = null,
          [CallerLineNumber] int callerLine = 0,
          [CallerMemberName] string callerMethod = null) => WriteLine(message, Levels.Erro, isDetailedMode, callerPath, callerLine, callerMethod);

        static readonly object _Lock = new object();
        private static void WriteLine(
            string message,
            Levels level,
            bool isDetailedMode,
            string callerPath,
            int callerLine,
            string callerMethod)
        {
            lock (_Lock)
            {
                string msg;
                if (isDetailedMode)
                    msg = DetailedMessage(message, level, callerPath, callerLine, callerMethod);
                else
                    msg = SimpleMessage(message, level);

                foreach (var log in LoggerDict.Values)
                {
                    log.WriteLine(msg, level);
                }
            }
        }

        private static string DetailedMessage(
            string message,
            Levels level,
            string callerPath,
            int callerLine,
            string callerMethod)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append(DateTime.Now.ToString() + " ");
            msg.Append($"[ {level} ] -> ");
            msg.Append($"{Path.GetFileName(callerPath)} > {callerMethod}() > in line[{callerLine.ToString().PadLeft(3, ' ')}]: ");

            msg.Append(message);
            return msg.ToString();
        }

        private static string SimpleMessage(string message, Levels level) => $"{DateTime.Now} [ {level} ] -> {message}";

        /// <summary>
        /// 打印一行log的具体实现
        /// </summary>
        /// <param name="msg">完整的消息内容</param>
        /// <param name="level"><see cref="Levels"/></param>
        protected abstract void WriteLine(string msg, Levels level = Levels.Dbug);

    }
}
