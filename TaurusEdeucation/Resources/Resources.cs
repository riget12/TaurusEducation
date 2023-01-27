using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Superpower.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace TaurusEdeucation.Resources
{
  public class Resources
  {
    public static string GetResourceFileAsStream(string fileName)
    {
      Assembly assembly = Assembly.GetExecutingAssembly();
      string result = String.Empty;

      using (var stream = assembly.GetManifestResourceStream(fileName))
      using (StreamReader reader = new StreamReader(stream))
      {
        result = reader.ReadToEnd();
      }

      return result;
    }
  }
}