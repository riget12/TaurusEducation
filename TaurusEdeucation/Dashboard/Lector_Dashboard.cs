using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Composing;
using Umbraco.Core.Dashboards;

namespace TaurusEdeucation.Dashboard
{
  [Weight(100)]
  public class LectorDashboard : IDashboard
  {
    public string[] Sections => new[]
    {
      Umbraco.Core.Constants.Applications.Members
    };

    public IAccessRule[] AccessRules => new[] { new AccessRule { Type = AccessRuleType.Grant, Value = Umbraco.Core.Constants.Security.AdminGroupAlias } };
   
    public string Alias => "Lektoři";

    public string View => "/App_Plugins/LectorDashboard/dashboard.html";
  }

  //[Weight(-10)]
  //public class MyDashboard : IDashboard
  //{
  //  public string Alias => "myCustomDashboard";

  //  public string[] Sections => new[]
  //  {
  //          Umbraco.Core.Constants.Applications.Content,
  //          Umbraco.Core.Constants.Applications.Settings
  //      };

  //  public string View => "/App_Plugins/myCustom/dashboard.html";

  //  public IAccessRule[] AccessRules => Array.Empty<IAccessRule>();
  //}
}