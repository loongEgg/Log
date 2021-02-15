using System;
using System.IO;
using System.Text;

namespace LoongEgg.Log
{
    /// <summary>
    /// 文件类型的<see cref="Logger"/>
    /// </summary>
    public class FileLogger : Logger
    {
        /// <summary>
        /// 文件完整路径和名称
        /// </summary>
        public string FileName { get; }

        internal FileLogger(Levels level) : base(level)
        {
            string root = Environment.CurrentDirectory;

            if (!Directory.Exists(root + @"/log/"))
            {
                Directory.CreateDirectory(root + @"/log/");
            }

            FileName = root + @"/log/" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".log";
            lock (_Lock)
            {
                using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                {

                }
            }
        }

        readonly object _Lock = new object();
         
        protected override void WriteLine(string msg, Levels level = Levels.Dbug)
        {
            lock (_Lock)
            {
                var buff = Encoding.Default.GetBytes(msg + Environment.NewLine);
                using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                {
                    fs.Seek(0, SeekOrigin.End);
                    fs.Write(buff, 0, buff.Length);
                }
            }
        }
    }
}