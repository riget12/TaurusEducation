using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TaurusEdeucation.Resources;

namespace TaurusEdeucation.Email_Sender
{
  public class Emails
  {
    public string GetEmailHTMLBody(string name, Dictionary<string, string> dict = null)
    {
      string htmlString = Resources.Resources.GetResourceFileAsStream(name);

      if (dict != null)
      {
        foreach (var item in dict)
        {
          htmlString = htmlString.Replace("{" + item.Key + "}", item.Value);
        }
      }

      return htmlString;
    }
    //public string GetStudentRegister(string name, string lesson, string place, string text, string phone, string mail)
    //{
    //  string HtmlString;
    //  string WorkingDirectory = Environment.CurrentDirectory;

    //  using (StreamReader sr = new StreamReader(WorkingDirectory + "/EmailsHtml/StudentRegister.html"))
    //  {
    //    HtmlString = sr.ReadToEnd();
    //  }

    //  HtmlString = HtmlString.Replace("{name}", name);
    //  HtmlString = HtmlString.Replace("{lesson}", lesson);
    //  HtmlString = HtmlString.Replace("{place}", place);
    //  HtmlString = HtmlString.Replace("{text}", text);
    //  HtmlString = HtmlString.Replace("{phone}", phone);
    //  HtmlString = HtmlString.Replace("{mail}", mail);

    //  return HtmlString;
    //}

    //public string GetLectorRegister()
    //{
    //  string HtmlString;
    //  string WorkingDirectory = Environment.CurrentDirectory;

    //  using (StreamReader sr = new StreamReader(WorkingDirectory + "/EmailsHtml/LectorRegister.html"))
    //  {
    //    HtmlString = sr.ReadToEnd();
    //  }

    //  HtmlString = HtmlString.Replace("{taurusMail}", config.AppSettings.EmailFrom);

    //  return HtmlString;
    //}
    //public string GetLectorRegister()
    //{
    //  string HtmlString;
    //  string WorkingDirectory = Environment.CurrentDirectory;

    //  using (StreamReader sr = new StreamReader(WorkingDirectory + "/EmailsHtml/LectorRegister.html"))
    //  {
    //    HtmlString = sr.ReadToEnd();
    //  }

    //  HtmlString = HtmlString.Replace("{taurusMail}", config.AppSettings.EmailFrom);

    //  return HtmlString;
    //}
  }
}