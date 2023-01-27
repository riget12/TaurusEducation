using System.Web.Mvc;
using System.Web.Security;
using TaurusEdeucation.Models;

namespace TaurusEdeucation.Controllers
{
  public class MemberLoginSurfaceController : Umbraco.Web.Mvc.SurfaceController
  {
    [HttpGet]
    [ActionName("MemberLogin")]
    public ActionResult MemberLoginGet()
    {
      return PartialView("MemberLogin", new MemberLoginModel());
    }

    [HttpGet]
    public ActionResult MemberLogout()
    {
      Session.Clear();
      FormsAuthentication.SignOut();
      return Redirect("/");
    }

    [HttpPost]
    [ActionName("MemberLogin")]
    public ActionResult MemberLoginPost(MemberLoginModel model)
    {
      if (ModelState.IsValid)
      {
        if (Membership.ValidateUser(model.Username, model.Password))
        {
          FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
          return RedirectToUmbracoPage(1160);
        }
        else
        {
          TempData["Status"] = "Neplatné přihlašovací jméno nebo heslo.";
          return RedirectToCurrentUmbracoPage();
        }
      }
      else
      {
        TempData["Status"] = "Vložte své přihlašovací jméno a heslo.";
        return RedirectToCurrentUmbracoPage();
      }
    }
  }
}