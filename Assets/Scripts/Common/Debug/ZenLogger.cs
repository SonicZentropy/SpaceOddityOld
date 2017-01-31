// /** 
//  * Logger.cs
//  * Will Hart
//  * 20161203
// */


// #define FORCE_LOG_TO_FILE

#if !UNITY_EDITOR || FORCE_LOG_TO_FILE
#define LOG_TO_FILE
#endif


    #region Dependencies

    using UnityEngine;
    using Zen.Common.Debug;

#if LOG_TO_FILE
    using NLog.Config;
    using NLog;
    using NLog.Targets;
#endif

    #endregion

    /// <summary>
    ///     A static class for logging to console or file through the same interface
    /// </summary>
    public static class ZenLogger
    {

#if LOG_TO_FILE
        private static readonly NLog.Logger NLogger;

        /// <summary>
        /// Set configuration for the logger manually as otherwise we would need
        /// to edit the configuration at run time anyway
        /// 
        /// NOTE: NLog 4.3.8 being used due to Authentication issues in https://github.com/NLog/NLog/issues/1644
        /// </summary>
        static ZenLogger()
        {
            Logengine.ThrowExceptions = true;
            Logengine.ThrowConfigExceptions = true;

            var config = new LoggingConfiguration();

            var target = new FileTarget("logfile")
            {
                MaxArchiveFiles = 3,
                ArchiveNumbering = ArchiveNumberingMode.Date,
                FileName= $"{Application.persistentDataPath}/debug.log",
                CreateDirs = true,
                KeepFileOpen = false,
                ConcurrentWrites = false, // http://stackoverflow.com/questions/40598922
                ForceManaged = true // http://stackoverflow.com/questions/20126526/
            };
            config.AddTarget("logfile", target);
            
            var rule = new LoggingRule("*", LogLevel.Trace, target);
            config.LoggingRules.Add(rule);

            Logengine.Configuration = config;
            NLogger = Logengine.GetCurrentClassLogger();
            
            Debug.Log($"Logging to file at {Application.persistentDataPath}/debug.log");
        }
#endif

	    public static void GameLog(object message)
	    {
		    InGameConsole.Instance.Print(message.ToString());
	    }

        public static void Log(object message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#endif
#if LOG_TO_FILE
            NLogger.Log(LogLevel.Info, message);
#endif
        }

        public static void LogWarning(object message)
        {
#if UNITY_EDITOR
            Debug.LogWarning(message);
#endif
#if LOG_TO_FILE
            NLogger.Log(LogLevel.Warn, message);
#endif
        }

        public static void LogError(object message)
        {
#if UNITY_EDITOR
            Debug.LogError(message);
#endif
#if LOG_TO_FILE
            NLogger.Log(LogLevel.Error, message);
#endif
        }
    }
namespace Zen.Common.Debug
{
}