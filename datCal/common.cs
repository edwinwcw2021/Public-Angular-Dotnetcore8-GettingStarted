using NLog;
using NLog.Config;
using NLog.Targets;

namespace datCal
{
  public class common
  {
    // NLog start
    private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

    public static void CreateLogger(string? logPath)
    {
      var config = new LoggingConfiguration();
      var fileTarget = new FileTarget
      {
        FileName = logPath + "/${shortdate}.log",
        Layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss} [${uppercase:${level}}] ${message}",
      };
      var fileTargetErr = new FileTarget
      {
        FileName = logPath + "/err${shortdate}.log",
        Layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss} [${uppercase:${level}}] ${message}",
      };
      config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Warn, fileTarget);
      config.AddRule(NLog.LogLevel.Error, NLog.LogLevel.Fatal, fileTargetErr);
      LogManager.Configuration = config;
    }

    public static void AppLog(string strMessage)
    {
      logger.Info(strMessage);
    }

    public static void ErrLog(string strMessage)
    {
      logger.Error(strMessage);
    }

  }
}
