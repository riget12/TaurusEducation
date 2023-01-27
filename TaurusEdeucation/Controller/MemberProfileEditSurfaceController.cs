using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using TaurusEdeucation.config;
using TaurusEdeucation.Models;
using TaurusEdeucation.Types;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace TaurusEdeucation.Controller
{
    public class MemberProfileEditSurfaceController : SurfaceController
    {
        MembershipProvider MembershipProvider;

        public MemberProfileEditSurfaceController(MembershipProvider membershipProvider)
        {
            MembershipProvider = membershipProvider;
        }

        /// <summary>
        /// nastaví druhotné hodnoty lectora podle současného membera
        /// </summary>
        /// <param name="model"></param>
        private void UpdateMember(MemberProfileEditModel model)
        {
            try
            {
                string email = Members.GetCurrentMember().GetProperty("email").GetValue().ToString();
                var memberService = Services.MemberService;
                var createdLector = memberService.GetByEmail(model.Email);
                                
                createdLector.SetValue("phone", model.Phone);
                createdLector.SetValue("resume", model.Resume);
                createdLector.SetValue("photo", model.Phone);

                memberService.Save(createdLector);
            }
            catch (Exception ex)
            {
                throw new Exception("Nastala chyba v procesu UpdateNewMember().\n", ex);
            }
        }

        /// <summary>
        /// vrátí partialview profile modelu pro změnu hesla
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MemberProfileEditPasswordGet()
        {
            return PartialView("MemberProfileEditPassword", new MemberProfileEditPasswordModel());
        }

        public ActionResult MemberProfileEditPasswordPost(MemberProfileEditPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                IMember member = Services.MemberService.GetByUsername(model.email);

                if (member == null)
                {
                    Logger.Error(GetType(), $"Objevila se nečekaná chyba: Nebyla");
                    return RedirectToCurrentUmbracoPage();
                }
                else
                {
                    try
                    {
                        if (model.password != null && model.password != "")
                        {
                            Umbraco.Web.Models.ChangingPasswordModel changingPasswordModel = new Umbraco.Web.Models.ChangingPasswordModel()
                            {
                                OldPassword = model.password,
                                NewPassword = model.newPassword
                            };

                            Members.ChangePassword(model.email, changingPasswordModel, MembershipProvider);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(GetType(), ex);
                        return RedirectToCurrentUmbracoPage();
                    }
                }
                return RedirectToUmbracoPage(1289);
            }

            return RedirectToCurrentUmbracoPage();
        }

        /// <summary>
        /// vrátí parrtialview profile modelu pro edit
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MemberProfileEditGet()
        {
            var member = Members.GetCurrentMember();

            MemberProfileEditModel model = new MemberProfileEditModel();
            model.FirstName = member.GetProperty("firstName").GetValue().ToString();
            model.SurName = member.GetProperty("surName").GetValue().ToString();
            model.City = member.GetProperty("city").GetValue().ToString();
            model.Street = member.GetProperty("street").GetValue().ToString();
            model.Phone = member.GetProperty("phone").GetValue().ToString();
            model.Email = member.GetProperty("email").GetValue().ToString();

            return PartialView("MemberProfileEdit", model);
        }

        /// <summary>
        /// zkontroluje správnost vyplnění registračního modelu a vytvoří nového lectora
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MemberProfileEditPost(MemberProfileEditModel model)
        {
            if (ModelState.IsValid)
            {
                var checkForRegisteredEmail = Services.MemberService.GetByUsername(model.Email);

                if (checkForRegisteredEmail == null || Members.CurrentUserName == model.Email)
                {
                    try
                    {
                        UpdateMember(model);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(GetType(), ex);
                        throw ex;
                    }
                    return RedirectToUmbracoPage(1289);
                }
                else
                {
                    TempData["RegistrationStatus"] = "E-mail je již zaregistrovaný.";
                    return RedirectToCurrentUmbracoPage();
                }
            }
            else
            {
                TempData["RegistrationStatus"] = "Vyplňte prosím všechny údaje.";
                return RedirectToCurrentUmbracoPage();
            }
        }


    }
}