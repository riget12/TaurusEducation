using Umbraco.Core;
using Umbraco.Core.Composing;

namespace TaurusEdeucation.Database.StudentLector
{
  public class StudentLectorComposer : IUserComposer
  {
    public void Compose(Composition composition)
    {
      composition.Components()
          .Append<StudentLectorComponent>();
    }
  }
}