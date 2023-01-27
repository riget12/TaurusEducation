using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace TaurusEdeucation.Validations
{
  /// <summary>
  /// kontrola, že první element není "Vyber"
  /// </summary>
  public class ValidateAnyElementOfArray : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if (value == null) { return ValidationResult.Success; }

      string[] ValuesInArray = (string[])value;
      
      foreach (string val in ValuesInArray)
      {
        if (val != "" && val != "Value")
        {
          return ValidationResult.Success;
        }
      }
      return new ValidationResult(base.ErrorMessageString);
    }
  }
}