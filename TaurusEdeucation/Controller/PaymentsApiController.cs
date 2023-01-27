using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.WebApi;

namespace TaurusEdeucation.Controller
{
  [System.Web.Http.Route("api/PaymentsApi")]
  public class PaymentsApiController : UmbracoApiController
  {
    [System.Web.Http.AcceptVerbs("POST")]
    public List<string> GetLessons()
    {
      var currentMember = Members.GetCurrentMember();
      List<string> output = new List<string>();

      output.Add("");
      for (int i = 0; i < 10; i++)
      {
        output.Add(currentMember.GetProperty("lesson" + i).GetValue().ToString());
      }

      return output;
    }

    [System.Web.Http.AcceptVerbs("POST")]
    public int GetDaysInMonth(int year, int month)
    {
      return DateTime.DaysInMonth(year, month);
    }

    [System.Web.Http.AcceptVerbs("POST")]
    public int GetDayInMonth()
    {
      return DateTime.Now.Day;
    }
  }
}