using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using TaurusEdeucation.JSON;

namespace TaurusEdeucation.Models
{
  public class PaymentsModel
  {
    public Manipulations manipulations { get; set; }

    //rok
    public int year { get; set; }

    //mesic
    public int month { get; set; }

    //ceny na dny
    public int[] pricesDays = new int[31];

  }
}