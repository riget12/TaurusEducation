using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using TaurusEdeucation.config;
using TaurusEdeucation.Database.Lector;
using TaurusEdeucation.helpers;
using TaurusEdeucation.Models;
using TaurusEdeucation.Models.SubModels;
using Umbraco.Core.Scoping;
using Umbraco.Web.WebApi;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace TaurusEdeucation.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/MemberApi")]
    public class MemberApiController : UmbracoApiController
    {
        private readonly IScopeProvider _scopeProvider;

        public MemberApiController(IScopeProvider scopeProvider)
        {
            this._scopeProvider = scopeProvider;
        }

        [HttpGet]
        public ActionResult GetAvailableLectors(string kraj, string okresy, int? lesson, int? level)
        {
            var output = new List<LectorDatabaseModel>();
            List<LectorViewModel> lectors = new List<LectorViewModel>();

            var members = Services.MemberService.GetAllMembers();

            members = members.Where(x => (bool)x.GetValue("isHidden") == false);

            if (!String.IsNullOrEmpty(kraj))
            {
                members = members.Where(x => x.GetValue("kraj").ToString() == kraj);
            }

            if (!String.IsNullOrEmpty(okresy))
            {
                string[] okresyArray = okresy.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var okres in okresyArray)
                {
                    members = members.Where(x => x.GetValue("okresy").ToString().Contains(okres));
                }
            }

            foreach (var member in members)
            {
                using (SqlConnection con = new SqlConnection(ConnectionStrings.UmbracoDbDSN))
                {
                    con.Open();
                    string sql = "select * from LectorLevelLessons where Lector_Id = @LectorId";
                    SqlCommand command = new SqlCommand();
                    command.Parameters.Add(new SqlParameter("@LectorId", member.Id));
                    command.Connection = con;

                    if (level.HasValue)
                    {
                        sql += "and Level_Id = @levelId";
                        command.Parameters.Add(new SqlParameter("@levelId", level.Value));
                    }
                    if (lesson.HasValue)
                    {
                        sql += "and Lesson_Id = @lessonId";
                        command.Parameters.Add(new SqlParameter("@lessonId", lesson.Value));
                    }

                    command.CommandText = sql;

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        LectorViewModel lector = new LectorViewModel();
                        lector.Id = member.Id;
                        lector.Name = member.Name;
                        string krajName = LocationHelper.GetKrajName(member.GetValue("kraj").ToString());

                        var okresyDb = member.GetValue("okresy");
                        string okresyDisplay = "";
                        if (okresyDb != null)
                        {
                            string[] okresyNames = LocationHelper.GetOkresyName(okresyDb.ToString().Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                            okresyDisplay = String.Join(", ", okresyNames);
                        }

                        lector.Kraj = krajName;
                        lector.Okresy = okresyDisplay;
                        lectors.Add(lector);
                    }
                }
            }

            // doplnění druhotných informací
            foreach (var lector in lectors)
            {
                List<LevelViewModel> levels = new List<LevelViewModel>();

                using (SqlConnection con = new SqlConnection(ConnectionStrings.UmbracoDbDSN))
                {
                    con.Open();

                    // získání úrovně a předmětů
                    string sqlLevel = "select Lector_Id, Level_Name, Lesson_Name from LectorLevelLessonView where Lector_Id = @lectorId";
                    SqlCommand sqlCommand = new SqlCommand(sqlLevel, con);
                    sqlCommand.Parameters.Add(new SqlParameter("@lectorId", lector.Id));

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

                        if (lector.Levels == null)
                        {
                            lector.Levels = new List<LevelViewModel>();
                        }
                        lector.Levels.Add(levelViewModel);
                    } 
                }
            }

            return new JsonResult()
            {
                Data = JsonConvert.SerializeObject(lectors),
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        [HttpGet]
        public LectorDisplayProfile GetLectorProfile(int id)
        {
            var lector = Services.MemberService.GetById(id);
            string[] arr = new string[0];

            LectorDisplayProfile output = new LectorDisplayProfile()
            {
                FirstName = lector.GetValue("firstName") == null ? "" : lector.GetValue("firstName").ToString(),
                LastName = lector.GetValue("surName") == null ? "" : lector.GetValue("surName").ToString(),
                //WorkPlace = lector.GetValue("placeOfWork") == null ? "" : GetLocation(lector.GetValue("placeOfWork").ToString()),
                Lessons = new string[0],
                Elementary = lector.GetValue("elementary").ToString() == "1",
                High = lector.GetValue("high").ToString() == "1",
                College = lector.GetValue("college").ToString() == "1",
                Resume = lector.GetValue("resume") == null ? "" : lector.GetValue("resume").ToString(),
                ThumbnailImagePath = lector.GetValue("photoSmallImage") == null ? "" : lector.GetValue("photoSmallImage").ToString()
            };

            for (int i = 0; i < 10; i++)
            {
                string lesson = lector.GetValue("lesson" + i) == null ? "" : lector.GetValue("lesson" + i).ToString();
                if (lesson != "")
                {
                    Array.Resize(ref arr, arr.Length + 1);
                    arr[i] = lesson;
                }
            }
            output.Lessons = arr;

            return output;
        }

        [HttpPost]
        public void SendMessageToLector(LectorDisplayMessage lectorDisplayMessage)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
      {
        { "name", lectorDisplayMessage.StudentName + " " + lectorDisplayMessage.StudentLastName },
        { "lesson", lectorDisplayMessage.Lesson },
        { "place", lectorDisplayMessage.Location },
        { "text", lectorDisplayMessage.Message },
        { "phone", lectorDisplayMessage.StudentPhone },
        { "mail", lectorDisplayMessage.StudentEmail }
      };

            string htmlBody = new Email_Sender.Emails().GetEmailHTMLBody("TaurusEdeucation.EmailSender.EmailsHtml.StudentRegister.html", keyValuePairs);

            _ = new Email_Sender.EmailSender(lectorDisplayMessage.Id, "Nový student má zájem", htmlBody);
        }

        [HttpPost]
        public void SendPaymentsToLector(int year, int month)
        {
            var lector = Members.GetCurrentMember();
            long amount = new JSON.Manipulations(lector.GetProperty("tableLessons").GetValue().ToString()).GetPaymentsAmount(year, month);


            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
      {
        { "amount", amount.ToString() },
        { "account", "DOPLNIT ÚČET" },
        { "dueDate", "DOPLNIT SPLATNOST" },
        { "variableSymbol", "DOPLNIT VARIABILNÍ SYMBOL" }
      };

            string htmlBody = new Email_Sender.Emails().GetEmailHTMLBody("TaurusEdeucation.EmailSender.EmailsHtml.Payments.html", keyValuePairs);

            _ = new Email_Sender.EmailSender(Services.MemberService.GetById(lector.Id).Email, "Informace k platbě", htmlBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelId"></param>
        [HttpGet]
        public JsonResult GetLessonsSelect(int levelId)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            using (SqlConnection con = new SqlConnection(ConnectionStrings.UmbracoDbDSN))
            {
                con.Open();

                string sql = $"select Id, Name"
                           + $"  from Lessons as l"
                           + $"  where l.Level_Id = @levelId";

                SqlCommand command = new SqlCommand(sql, con);
                command.Parameters.Add(new SqlParameter("@levelId", levelId));

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string id = reader.GetInt64(0).ToString();
                    string name = reader.GetString(1);

                    result.Add(id, name);
                }
            }

            return new JsonResult()
            {
                Data = JsonConvert.SerializeObject(result.Select(x => new { Id = x.Key, Name = x.Value })),
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }
    }
}
