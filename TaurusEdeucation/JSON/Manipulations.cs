using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaurusEdeucation.Models;

namespace TaurusEdeucation.JSON
{
  public class Manipulations
  {
    private JObject jObject;

    public Manipulations(string jString)
    {
      if (jString.Contains(@"""backgroundColor"": ""none"""))
      {
        jObject = JObject.Parse("{}");
      }
      else
      {
        jObject = JObject.Parse(jString);
      }
    }

    /// <summary>
    /// Check na existenci struktury
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool Check(string name)
    {
      //jObject = jObject == null ? new JObject() : jObject;

      return name == null ? false : jObject[name] != null;
    }
    public bool Check(string name, int year)
    {
      if (Check(name))
      {
        var temp = jObject[name];

        foreach (var item in temp)
        {
          if (new JObject(item).ContainsKey(Convert.ToString(year)))
          {
            return true;
          }
        }
      }

      return false;
    }
    public bool Check(string name, int year, int month)
    {
      if (Check(name, year))
      {
        var temp = jObject[name][Convert.ToString(year)];

        foreach (var item in temp)
        {
          if (new JObject(item).ContainsKey(Convert.ToString(month)))
          {
            return true;
          }
        }
      }

      return false;
    }

    public JObject GetJObject()
    {
      return jObject;
    }

    public int[] GetPayments(int year, int month)
    {
      int[] output = new int[31];

      if (jObject.Count == 0)
      {
        return output;
      }

      foreach (var item in jObject)
      {
        if (Check(item.Key, year, month))
        {
          foreach (var lesson in item.Value[year.ToString()][month.ToString()])
          {
            output[lesson.Value<int>("day") - 1] += lesson.Value<string>("price") == null ? 0 : lesson.Value<int>("price");
          }
        }
      }

      return output;
    }

    public long GetPaymentsAmount(int year, int month)
    {
      long output = 0;

      if (jObject.Count == 0)
      {
        return output;
      }

      foreach (var item in jObject)
      {
        if (Check(item.Key, year, month))
        {
          var jItem = new JObject(item);
          foreach (var lesson in jItem[year.ToString()][month.ToString()])
          {
            var jLesson = new JObject(lesson);
            output += Convert.ToInt32(jLesson["price"]);
          }
        }
      }

      return output;
    }

    public List<SelectListItem> GetStudents()
    {
      List<SelectListItem> output = new List<SelectListItem>();

      foreach (var item in jObject)
      {
        output.Add(new SelectListItem { Text = item.Key, Value = item.Key });
      }

      return output;
    }

    public void InsertStudent(string name)
    {
      if (jObject.Count == 0)
      {
        jObject = new JObject(new JProperty(name, ""));
      }
      else
      {
        jObject.Last.AddAfterSelf(new JProperty(name, ""));
      }
    }

    public JToken[] GetDaysJToken(string name, int year, int month)
    {
      if (Check(name, year, month))
      {
        return jObject[name][Convert.ToString(year)][Convert.ToString(month)].ToArray();

      }
      return null;
    }

    public DayModel[] GetDays(string name, int year, int month)
    {
      DayModel[] output = new DayModel[31];

      for (int i = 0; i < 31; i++)
      {
        output[i] = new DayModel();
      }

      if (Check(name, year, month))
      {
        var tempArr = jObject[name][Convert.ToString(year)][Convert.ToString(month)].ToArray();

        foreach (var item in tempArr)
        {
          int day = Convert.ToInt32(item["day"].ToString())-1;

          output[day].hour = item["hour"].ToString();
          output[day].price = item["price"].ToString();
        }
      }
      return output;
    }

    public void ReplaceMonth(string name, int year, int month, DayModel[] input)
    {
      if (name == null)
      {
        return;
      }

      JObject jToken = GenerateJToken(month, input);

      if (Check(name))
      {
        if (Check(name, year))
        {
          if (Check(name, year, month))
          {
            jObject[name][Convert.ToString(year)][Convert.ToString(month)].Replace(jToken[Convert.ToString(month)]);
          }
          else
          {
            if (jObject[name][Convert.ToString(year)].Count() == 0)
            {
              jObject[name][Convert.ToString(year)] = new JObject(jToken);
            }
            else
            {
              jObject[name][Convert.ToString(year)].Last.AddAfterSelf(jToken.First);
            }
          }
        }
        else
        {
          jToken = new JObject(new JProperty(Convert.ToString(year), new JObject(jToken)));
          if (jObject[name].Count() == 0)
          {
            jObject[name] = new JObject(jToken);
          }
          else
          {
            jObject[name].Last.AddAfterSelf(jToken.First);
          }
        }
      }
      else
      {
        jToken = new JObject(new JProperty(Convert.ToString(year), new JObject(jToken)));
        if (jObject.Count == 0)
        {
          jObject = new JObject(new JProperty(name, new JObject(jToken)));
        }
        else
        {
          jObject.Last.AddAfterSelf(jToken.First);
        }
      }
    }

    private JObject GenerateJToken(int month, DayModel[] input)
    {
      JObject output = new JObject(new JProperty(Convert.ToString(month), new JValue("")));

      for (int i = 0; i < 31; i++)
      {
        if (input[i].price != null)
        {
          if (output[Convert.ToString(month)].Count() == 0)
          {
            output[Convert.ToString(month)] = new JArray(new JObject(new JProperty("day", new JValue(i+1)), new JProperty("hour", new JValue(input[i].hour)), new JProperty("price", new JValue(input[i].price))));
          }
          else
          {
            output[Convert.ToString(month)].Last.AddAfterSelf(new JObject(new JProperty("day", new JValue(i+1)), new JProperty("hour", new JValue(input[i].hour)), new JProperty("price", new JValue(input[i].price))));
          }
        }
      }
      return output;
    }
  }
}