using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaurusEdeucation.Models
{
  public class LectorDisplayMessage
  {
    public string Id { get; set; }

    public string Message { get; set; }

    public string StudentName { get; set; }
    public string StudentLastName { get; set; }
    public string StudentEmail { get; set; }
    public string StudentPhone { get; set; }
    public string LevelsOfStudy { get; set; }
    public string Lesson { get; set; }
    public string Location { get; set; }
  }
}