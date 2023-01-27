using System;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using TaurusEdeucation.Models;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.Models;
using Umbraco.Core.Scoping;
using System.Collections.Generic;
using System.Data.SqlClient;
using TaurusEdeucation.config;
using System.Data;

namespace TaurusEdeucation.Controllers
{
    /// <summary>
    /// SurfaceController pro registrační formulář
    /// </summary>
    public class MemberRegisterSurfaceController : SurfaceController
    {
        private readonly IScopeProvider _scopeProvider;

        public MemberRegisterSurfaceController(IScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }

        /// <summary>
        /// zkontroluje správnost vyplnění registračního modelu a vytvoří nového lectora
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberSubmit(MemberRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (CheckIfEmailWasDeleted(model.Email))
                {
                    TempData["RegistrationStatus"] = "E-mail byl už v minulosti registrován. Obraťte se prosím na správce systému.";
                    TempData["Success"] = false;
                    return CurrentUmbracoPage();
                }

                IMember checkForRegisteredEmail = Services.MemberService.GetByEmail(model.Email);
                int lectorId = 0;

                if (checkForRegisteredEmail == null)
                {
                    try
                    {
                        CreateNewMember(model);
                        lectorId = GetNewMemberId(model);
                        UpdateNewMember(model, lectorId);

                        //SendEmail(model.Email);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(GetType(), $"During register of user we encountered error: {ex.Message}");
                        TempData["RegistrationStatus"] = "Registrace se nepovedla";
                        TempData["Success"] = false;
                        return CurrentUmbracoPage();
                    }
                    ViewBag.LectorId = lectorId;

                    TempData["RegistrationStatus"] = "Succesful Registration";
                    TempData["Success"] = true;
                    return CurrentUmbracoPage();
                }
                else
                {
                    TempData["RegistrationStatus"] = "E-mail je již zaregistrovaný.";
                    TempData["Success"] = false;
                    return CurrentUmbracoPage();
                }
            }
            else
            {
                TempData["RegistrationStatus"] = "Vyplňte prosím všechny údaje.";
                TempData["Success"] = false;
                return CurrentUmbracoPage();
            }
        }

        /// <summary>
        /// nastaví druhotné hodnoty lectora podle současného registračního modelu
        /// </summary>
        /// <param name="model"></param>
        private void UpdateNewMember(MemberRegisterModel model, int lektorId)
        {
            try
            {
                IMemberService memberService = Services.MemberService;
                IMember createdLector = memberService.GetByEmail(model.Email);

                string okresyString = model.Okres;
                if (!String.IsNullOrEmpty(okresyString))
                {
                    var okresy = okresyString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    okresyString = String.Join(";", okresy);
                }
                
                createdLector.SetValue("firstName", model.FirstName);
                createdLector.SetValue("surName", model.SurName);
                createdLector.SetValue("city", model.City);
                createdLector.SetValue("street", model.Street);
                createdLector.SetValue("phone", model.Phone);
                createdLector.SetValue("kraj", model.Kraj);
                createdLector.SetValue("okresy", okresyString);
                createdLector.IsApproved = false;

                Dictionary<string, List<string>> levelLessons = new Dictionary<string, List<string>>();

                for(int i = 0; i < model.Levels.Length; i++)
                {
                    string level = model.Levels[i];

                    if (levelLessons.ContainsKey(level))
                    {
                        levelLessons[level].Add(model.Lessons[i]);
                    }
                    else
                    {
                        levelLessons.Add(level, new List<string>() { model.Lessons[i] });
                    }
                }

                using (SqlConnection con = new SqlConnection(ConnectionStrings.UmbracoDbDSN))
                {
                    con.Open();

                    foreach (var item in levelLessons)
                    {
                        foreach (var levelLesson in item.Value)
                        {
                            string sql = $"[dbo].[SaveLectorsLessons]";
                            SqlCommand command = new SqlCommand(sql, con)
                            {
                                CommandType = CommandType.StoredProcedure
                            };

                            command.Parameters.Add(new SqlParameter("@lectorId", lektorId) { SqlDbType = SqlDbType.BigInt });
                            command.Parameters.Add(new SqlParameter("@lessonId", levelLesson) { SqlDbType = SqlDbType.BigInt });
                            command.Parameters.Add(new SqlParameter("@levelId", item.Key) { SqlDbType = SqlDbType.BigInt });

                            command.ExecuteNonQuery();
                        }
                    }
                }

                memberService.Save(createdLector);
            }
            catch (Exception ex)
            {
                throw new Exception("Nastala chyba v procesu UpdateNewMember().\n", ex);
            }
        }

        /// <summary>
        /// Vytvoří nového lectora podle současného registračního modelu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private void CreateNewMember(MemberRegisterModel model)
        {
            try
            {
                RegisterModel lector = Members.CreateRegistrationModel("Lectors");

                lector.Name = model.FirstName + " " + model.SurName;
                lector.Email = model.Email;
                lector.Password = model.Password;
                lector.UsernameIsEmail = true;
                lector.LoginOnSuccess = false;

                Members.RegisterMember(lector, out _, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Nastala chyba v procesu CreateNewMember().\n", ex);
            }
        }

        /// <summary>
        /// Funkce pro získání Id Membera podle jeho email
        /// </summary>
        /// <param name="model">aktuální model</param>
        /// <returns>Id Membera</returns>
        private int GetNewMemberId(MemberRegisterModel model)
        {
            int result = 0;

            try
            {
                IMemberService memberService = Services.MemberService;
                IMember createdLector = memberService.GetByEmail(model.Email);

                result = createdLector.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Nastala chyba v procesu '{nameof(GetNewMemberId)}'.", ex);
            }

            return result;
        }

        private void SendEmail(string email)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
              {
                { "taurusMail", config.AppSettings.EmailFrom }
              };

            string HtmlString = new Email_Sender.Emails().GetEmailHTMLBody("TaurusEdeucation.EmailSender.EmailsHtml.LectorRegister.html", keyValuePairs);

            _ = new Email_Sender.EmailSender(email, "Dokončení registrace", HtmlString);
        }

        private bool CheckIfEmailWasDeleted(string email)
        {
            using (var con = new SqlConnection(ConnectionStrings.UmbracoDbDSN))
            {
                con.Open();

                string sql = "select * from LectorDeletedList where email = @email";
                SqlCommand command = new SqlCommand(sql, con);

                command.Parameters.Add(new SqlParameter("@email", email));

                var reader = command.ExecuteReader();

                return reader.HasRows;
            }
        }
    }
}