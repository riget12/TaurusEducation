using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TaurusEdeucation.config
{
  public class ConnectionStrings
  {
    public static string UmbracoDbDSN
    {
      get { return Setting("umbracoDbDSN"); }
    }

    private static string Setting(string name)
    {
      ConnectionStringSettings value = ConfigurationManager.ConnectionStrings[name];

      if (value == null)
      {
        throw new Exception($"Could not find setting '{name}'.");
      }

      return value.ConnectionString;
    }
  }
}