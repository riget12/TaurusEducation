using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using TaurusEdeucation.JSON;

namespace TaurusEdeucation.Models
{
  public class OverviewModel
  {
    
    public Manipulations manipulations { get; set; }

    public string newStudentName { get; set; }

    public string newStudentLesson { get; set; }

    public string active { get; set; }

    public string studentName { get; set; }

    //jmeno studenta
    public List<SelectListItem> studentNames = new List<SelectListItem>();

    //seznam predmetu
    public List<SelectListItem> listOfLessons = new List<SelectListItem>();

    //rok
    public int year { get; set; } 
    
    //mesic
    public int month { get; set; }

    //lekce
    public string allLessons { get; set; }

    //status studenta
    public Status status = new Status();

    //list lekcí
    public DayModel[] lessonsList { get; set; }

    public void SaveLessons()
    {
      manipulations.ReplaceMonth(studentName, year, month, lessonsList);
    }
  }

  public class DayModel
  {
    public string hour { get; set; }

    public string price { get; set; }
  }
}