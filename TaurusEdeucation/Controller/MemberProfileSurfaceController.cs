using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Windows.Forms;
using TaurusEdeucation.config;
using TaurusEdeucation.Database.Student;
using TaurusEdeucation.Models;
using TaurusEdeucation.Models.SubModels;
using Umbraco.Core.Scoping;
using Umbraco.Web.Mvc;

namespace TaurusEdeucation.Controller
{
    public class MemberProfileSurfaceController : SurfaceController
    {
        private readonly IScopeProvider scopeProvider;

        public MemberProfileSurfaceController(IScopeProvider scopeProvider)
        {
            this.scopeProvider = scopeProvider;
        }
        private StudentDatabaseModel FillDataRow()
        {
            StudentDatabaseModel output = new StudentDatabaseModel();

            output.email = "email@seznam.cz";
            output.level = "low one";
            output.location = "anywhere";
            output.name = "some name";
            output.phone = "phone";

            return output;
        }

        private void Insert()
        {
            using (var scope = scopeProvider.CreateScope())
            {
                scope.Database.Insert<StudentDatabaseModel>(FillDataRow());

                // Scope.Database has what you need/want
                scope.Database.Fetch<StudentDatabaseModel>("Select * From StudentDatabaseTable");

                // You must always complete a scope
                scope.Complete();
            }
        }

        /// <summary>
        /// vyplní overviewmodel s hodnotami současně připojeného membera
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private MemberProfileModel PopulateProfileModel(MemberProfileModel model)
        {
            var currentMember = Members.GetCurrentMember();

            model.Id = currentMember.Id;
            model.FirstName = currentMember.GetProperty("firstName").GetValue().ToString();
            model.SurName = currentMember.GetProperty("surName").GetValue().ToString();
            model.City = currentMember.GetProperty("city").GetValue().ToString();
            model.Street = currentMember.GetProperty("street").GetValue().ToString();
            model.Phone = currentMember.GetProperty("phone").GetValue().ToString();
            model.Email = currentMember.GetProperty("email").GetValue().ToString();

            using (var con = new SqlConnection(ConnectionStrings.UmbracoDbDSN))
            {
                con.Open();

                string sql = "select Lector_Id, Level_Name, Lesson_Name from LectorLevelLessonView where Lector_Id = @lectorId";
                SqlCommand sqlCommand = new SqlCommand(sql, con);
                sqlCommand.Parameters.Add(new SqlParameter("@lectorId", currentMember.Id));

                SqlDataReader reader = sqlCommand.ExecuteReader();

                Dictionary<string, List<string>> levelLessons = new Dictionary<string, List<string>>();
                while (reader.Read())
                {
                    string level2 = reader.GetString(1);
                    string lessonDisplay = reader.GetString(2).ToString();
                    if (!levelLessons.ContainsKey(level2))
                    {
                        levelLessons.Add(level2, new List<string>() { lessonDisplay });
                    }
                    else
                    {
                        levelLessons[level2].Add(lessonDisplay);
                    }

                }

                foreach (var item in levelLessons)
                {
                    LevelViewModel levelViewModel = new LevelViewModel();
                    levelViewModel.Name = item.Key;
                    levelViewModel.Lessons = new List<LessonViewModel>();

                    foreach (var ival in item.Value)
                    {
                        levelViewModel.Lessons.Add(new LessonViewModel() { Name = ival });
                    }

                    if (model.LevelViews == null)
                    {
                        model.LevelViews = new List<LevelViewModel>();
                    }
                    model.LevelViews.Add(levelViewModel);
                }
            }

            return model;
        }


        /// <summary>
        /// vrátí partialview profile modelu
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MemberProfileGet()
        {
            return PartialView("MemberProfile", PopulateProfileModel(new MemberProfileModel()));
        }

        /// <summary>
        /// zkontroluje správnost vyplnění registračního modelu a vytvoří nového lectora
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MemberProfilePost(MemberProfileModel model)
        {
            Insert();
            return RedirectToUmbracoPage(1294);
        }
    }
}