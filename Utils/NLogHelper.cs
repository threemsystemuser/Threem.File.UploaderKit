using NLog;
using NLog.Config;
using NLog.Targets;
using System.IO;

namespace Threem.File.UploaderKit.Utils
{
    public static class NLogHelper
    {
        public static LoggingConfiguration ConfigureNLog(string logDirectory)
        {
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget("logfile")
            {
                FileName = Path.Combine(logDirectory, "log.txt"),
                Layout = "${longdate}|${level}|${logger}|${message}|${exception}",
                CreateDirs = true
            };

            config.AddTarget(fileTarget); // <- this was missing
            config.AddRule(LogLevel.Info, LogLevel.Fatal, fileTarget);

            return config;
        }
    }
}
