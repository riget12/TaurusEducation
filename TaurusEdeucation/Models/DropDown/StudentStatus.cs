using System.Collections.Generic;
using System.Web.Mvc;

namespace TaurusEdeucation.Models
{
  public class Status
  {
    //string pro activeList
    public string output { get; set; }

    //seznam aktivností
    public List<SelectListItem> list = new List<SelectListItem>()
    {
      new SelectListItem {Text = "aktivní", Value = "active"},
      new SelectListItem {Text = "přerušen", Value = "paused"},
      new SelectListItem {Text = "ukončen", Value = "ended"}
    };
  }
}