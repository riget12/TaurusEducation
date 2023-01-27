using System.Web.Mvc;
using TaurusEdeucation.Models;
using Umbraco.Core.Scoping;
using Umbraco.Web.Mvc;

namespace TaurusEdeucation.Controller
{
    public class StudentRegisterSurfaceController : SurfaceController
    {
        private readonly IScopeProvider scopeProvider;

        public StudentRegisterSurfaceController(IScopeProvider scopeProvider)
        {
            this.scopeProvider = scopeProvider;
        }

        [HttpGet]
        [ActionName("StudentRegister")]
        public ActionResult StudentRegisterGet()
        {
            return PartialView("StudentRegister", new StudentRegisterModel());
        }

        [HttpPost]
        public ActionResult StudentRegisterPost(StudentRegisterModel model)
        {
            if (ModelState.IsValid)
            {


                return RedirectToUmbracoPage(1148);
            }
            else
            {
                TempData["RegistrationStatus"] = "Vyplňte prosím všechny údaje.";
                return CurrentUmbracoPage();
            }
        }
    }
}