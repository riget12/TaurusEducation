using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace TaurusEdeucation.Database.Student
{
  public class StudentComposer : IUserComposer
  {
    public void Compose(Composition composition)
    {
      composition.Components()
          .Append<StudentComponent>();
    }
  }
}