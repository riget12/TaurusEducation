using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;

namespace TaurusEdeucation.config
{
  public class AppSettings
  {
    public static string EmailSmtp
    {
      get { return Setting<string>("EmailSmtp"); }
    }
    public static int EmailPort
    {
      get { return Setting<int>("EmailPort"); }
    }
    public static string EmailUser
    {
      get { return Setting<string>("EmailUser"); }
    }
    public static string EmailPassword
    {
      get { return Setting<string>("EmailPassword"); }
    }
    public static string EmailTo
    {
      get { return Setting<string>("EmailTo"); }
    }
    public static string EmailFrom
    {
      get { return Setting<string>("EmailFrom"); }
    }
    public static int ProfileImagesMediaId
    {
      get { return Setting<int>("ProfileImagesMediaId"); }
    }

    private static T Setting<T>(string name)
    {
      string value = ConfigurationManager.AppSettings[name];

      if (value == null)
      {
        throw new Exception($"Could not find setting '{name}'.");
      }

      return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
    }

    private static string[] Setting(string name, string delimiter)
    {
      string value = Setting<string>(name);

      return value.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
    }

    private static Dictionary<string, string> SettingDictionary(string name, string delimiter)
    {
      string value = Setting<string>(name);
      string[] values = value.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
      Dictionary<string, string> result = new Dictionary<string, string>();

      foreach (var item in values)
      {
        string[] p = item.Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
        result.Add(p[0], p[1]);
      }

      return result;
    }
  }
}