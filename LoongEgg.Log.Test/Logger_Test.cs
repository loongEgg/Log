using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace LoongEgg.Log.Test
{
    [TestClass]
    public class Logger_Test
    {
        [TestMethod]
        public void EachLogger_IsSingletonInDictionary()
        {
            Logger.Clear();

            Logger.Enable(Loggers.Console ^ Loggers.Debug ^ Loggers.File);
            Logger.Enable(Loggers.Console ^ Loggers.Debug);

            Assert.IsTrue(Logger.LoggerDict.Count == 3);
        }

        [TestMethod]
        public void FileLogger_CanCreateFile()
        {
            Logger.Clear();
            Logger.Enable(Loggers.File);

            Assert.IsTrue(File.Exists((Logger.LoggerDict[Loggers.File] as FileLogger).FileName));

        }

        [TestMethod]
        public void FileLogger_CanWriteLog()
        {

            Logger.Clear();
            Logger.Enable(Loggers.File);
            Logger.Dbug("a debug message", true);
            Logger.Info("a info message", true);
            Logger.Warn("a warn message", true);
            Logger.Erro("a erro message", true);

            string fileName = (Logger.LoggerDict[Loggers.File] as FileLogger).FileName;
            Assert.IsTrue(File.Exists(fileName));

            using (StreamReader fs = new StreamReader(fileName))
            {
                var log = fs.ReadToEnd();
                Assert.IsTrue(log.Contains("debug"));
                Assert.IsTrue(log.Contains("info"));
                Assert.IsTrue(log.Contains("warn"));
                Assert.IsTrue(log.Contains("erro"));
            }
        }

    }
}
