using System;
using System.Web.Mvc;
using System.Web.Services;
using TaurusEdeucation.Database.StudentLector;
using TaurusEdeucation.Models;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Scoping;
using Umbraco.Web.Mvc;

namespace TaurusEdeucation.Controller
{
  public class OverviewSurfaceController : SurfaceController
  {
    private readonly IScopeProvider scopeProvider;

    public OverviewSurfaceController(IScopeProvider scopeProvider)
    {
      this.scopeProvider = scopeProvider;
    }
    private void Update(OverviewModel model)
    {
      var currentMemberEmail = Members.GetCurrentMember().GetProperty("email").GetValue().ToString();
      var currentLector = Services.MemberService.GetByEmail(currentMemberEmail);
      var data = currentLector.GetValue("tableLessons") == null ? "{}" : currentLector.GetValue("tableLessons").ToString();
      model.manipulations = new JSON.Manipulations(data);

      model.SaveLessons();

      currentLector.SetValue("tableLessons", model.manipulations.GetJObject());

      Services.MemberService.Save(currentLector);
    }

    [HttpGet]
    [ActionName("Overview")]
    public ActionResult OverviewGet()
    {
      IPublishedContent currentMember = Members.GetCurrentMember();

      if (currentMember == null)
      {
        return RedirectToUmbracoPage(1286);
      }
      string currentMemberEmail = currentMember.GetProperty("email").GetValue().ToString();
      IMember currentLector = Services.MemberService.GetByEmail(currentMemberEmail);
      string data = currentLector.GetValue("tableLessons") == null ? "{}" : currentLector.GetValue("tableLessons").ToString();
      OverviewModel model = new OverviewModel();
      JSON.Manipulations manipulations = new JSON.Manipulations(data);
      model.studentNames = manipulations.GetStudents();

      model.listOfLessons.Add(new SelectListItem { Value = "", Text = "Vyber"});
      for (int i = 0; i < 10; i++)
      {
        string lesson = currentLector.GetValue("lesson" + i) == null ? "" : currentLector.GetValue("lesson" + i).ToString();
        model.listOfLessons.Add(new SelectListItem { Value = lesson, Text = lesson });
      }

      return PartialView("Overview", model);
    }

    [HttpPost]
    [ActionName("Overview")]
    public ActionResult OverviewPost(OverviewModel model)
    {
      Update(model);

      return RedirectToCurrentUmbracoPage();
    }
  }
}