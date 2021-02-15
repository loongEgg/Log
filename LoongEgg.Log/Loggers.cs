using System;

namespace LoongEgg.Log
{
    /// <summary>
    /// <see cref="Logger"/>的类型枚举, 可以使用或"|"
    /// </summary>
    [Flags]
    public enum Loggers
    {
        /// <summary>
        /// 调试器显示
        /// </summary>
        Debug = 1,

        /// <summary>
        /// 控制台显示
        /// </summary>
        Console = 1 << 1,

        /// <summary>
        /// 文件记录
        /// </summary>
        File = 1 << 2
    }
}
