using Umbraco.Core;
using Umbraco.Core.Composing;

namespace TaurusEdeucation.Database.Lector
{
  public class LectorComposer : IUserComposer
  {
    public void Compose(Composition composition)
    {
      composition.Components()
          .Append<LectorComponent>();
    }
  }
}