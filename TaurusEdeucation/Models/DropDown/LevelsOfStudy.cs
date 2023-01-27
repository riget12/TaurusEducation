using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaurusEdeucation.Models
{
  /// <summary>
  /// úrovně studií
  /// </summary>
  public class LevelsOfStudy
  {
    public bool elementarySchool { get; set; }
    public bool highSchool { get; set; }
    public bool college { get; set; }
  }
}