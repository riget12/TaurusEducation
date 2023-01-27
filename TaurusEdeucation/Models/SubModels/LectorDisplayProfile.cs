using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaurusEdeucation.Models
{
  public class LectorDisplayProfile
  {
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string WorkPlace { get; set; }
    public string[] Lessons { get; set; }
    public bool Elementary { get; set; }
    public bool High { get; set; }
    public bool College { get; set; }
    public string Resume { get; set; }
    public string ThumbnailImagePath { get; set; }
  }
}