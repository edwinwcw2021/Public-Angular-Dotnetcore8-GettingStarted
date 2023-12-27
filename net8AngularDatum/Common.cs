using datCal;
namespace net8AngularDatum
{
  public static class Common
  {
    public static string getAppSetting(IConfiguration configuration, string name)
    {
      string? obj = configuration[name];
      return (obj == null) ? string.Empty : obj.ToString();
    }

    public static void CreateLogger(IConfiguration configuration)
    {
      common.CreateLogger(Common.getAppSetting(configuration, "logPath"));
    }

    public static void AppLog(string message)
    {
      common.AppLog(message);
    }

    public static void ErrLog(string message)
    {
      common.ErrLog(message);
    }

  }
}