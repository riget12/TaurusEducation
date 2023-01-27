using System;

namespace TaurusEdeucation.Models
{
  public class MemberProfileModel : MemberRegisterModel
  {
    public int Id { get; set; }
    public MemberProfileModel()
    {
      FirstName = "";
      SurName = "";
      City = "";
      Street = "";
      Phone = "";
      Lessons = new string[10];
      Levels = new string[10];
      Resume = String.Empty;
    }
  }
}