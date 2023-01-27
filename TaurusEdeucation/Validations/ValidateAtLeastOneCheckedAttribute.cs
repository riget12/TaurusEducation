using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace TaurusEdeucation.Validations
{
  /// <summary>
  /// kontrola, že alespoň jedna hodnota je true
  /// </summary>
  public class ValidateAtLeastOneCheckedAttribute : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if (value == null) { return ValidationResult.Success; }
      Type type = value.GetType();
      IEnumerable<PropertyInfo> checkBoxeProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.PropertyType == typeof(bool));

      foreach (PropertyInfo checkBoxProperty in checkBoxeProperties)
      {
        var isChecked = (bool)checkBoxProperty.GetValue(value);
        if (isChecked)
        {
          return ValidationResult.Success;
        }
      }

      return new ValidationResult(base.ErrorMessageString);
    }
  }
}