using NPoco;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Mvc;
using TaurusEdeucation.config;
using TaurusEdeucation.Database.StudentLector;
using TaurusEdeucation.Models;
using Umbraco.Core.Scoping;
using Umbraco.Web.WebApi;

namespace TaurusEdeucation.Controller
{
  [System.Web.Http.Route("api/OverviewApi")]
  public class OverviewApiController : UmbracoApiController
  {
    private readonly IScopeProvider scopeProvider;

    public OverviewApiController(IScopeProvider scopeProvider)
    {
      this.scopeProvider = scopeProvider;
    }

    [System.Web.Http.AcceptVerbs("POST")]
    public string GetActiveStatus(string name)
    {
      string email = Members.CurrentUserName;
      string output;

      using (var scope = scopeProvider.CreateScope().Database)
      {
        var fetch = scope.Fetch<StudentLectorDatabaseModel>("SELECT * FROM StudentLectorDatabaseTable WHERE LECTOREMAIL=" + email + " AND STUDENTNAME=" + name);
        output = fetch.Count > 0 ? fetch[0].active : "active";
      }

      return output;
    }

    [System.Web.Http.AcceptVerbs("POST")]
    public List<string> GetLessons()
    {
      var currentMember = Members.GetCurrentMember();
      List<string> output = new List<string>();

      output.Add("");
      for (int i = 0; i < 10; i++)
      {
        output.Add(currentMember.GetProperty("lesson"+i).GetValue().ToString());
      }

      return output;
    }
    [System.Web.Http.AcceptVerbs("POST")]
    public int GetDaysInMonth(int year, int month)
    {
      return DateTime.DaysInMonth(year, month);
    }

    [System.Web.Http.AcceptVerbs("POST")]
    public DayModel[] GetData(string name, int year, int month)
    {
      var currentMemberEmail = Members.GetCurrentMember().GetProperty("email").GetValue().ToString();
      var currentLector = Services.MemberService.GetByEmail(currentMemberEmail);
      var data = currentLector.GetValue("tableLessons") == null ? "{}" : currentLector.GetValue("tableLessons").ToString();
      JSON.Manipulations manipulations = new JSON.Manipulations(data);

      return manipulations.GetDays(name, Convert.ToInt32(year), Convert.ToInt32(month));
    }

    [System.Web.Http.AcceptVerbs("POST")]
    public void SaveStudent(string name, string lesson)
    {
      string currentMemberEmail = Members.GetCurrentMember().GetProperty("email").GetValue().ToString();
      var currentLector = Services.MemberService.GetByEmail(currentMemberEmail);
      var data = currentLector.GetValue("tableLessons") == null ? "{}" : currentLector.GetValue("tableLessons").ToString();
      JSON.Manipulations manipulations = new JSON.Manipulations(data);
      manipulations.InsertStudent(name + " - " + lesson);

      currentLector.SetValue("tableLessons", manipulations.GetJObject());
      Services.MemberService.Save(currentLector);

      StudentLectorDatabaseModel dataModel = new StudentLectorDatabaseModel()
      {
        active = "active",
        lectorEmail = Members.GetCurrentMember().Id.ToString(),
        studentName = name + " - " + lesson
      };

      using (SqlConnection con = new SqlConnection(ConnectionStrings.UmbracoDbDSN))
      {
        con.Open();
        string sql =  $"SELECT ID FROM StudentLectorDatabaseTable WHERE LECTOREMAIL = @currentLector AND STUDENTNAME = @name";

        SqlCommand command = new SqlCommand(sql, con);

        command.Parameters.Add(new SqlParameter("@currentLector", currentLector.Id));
        command.Parameters.Add(new SqlParameter("@name", name));

        SqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
          reader.Close();
          string updateSql = $"UPDATE StudentLectorDatabasTable SET ACTIVE = @active WHERE LECTOREMAIL = @currentLector AND STUDENTNAME = @name";

          SqlCommand updateCommand = new SqlCommand(updateSql, con);

          updateCommand.Parameters.Add(new SqlParameter("@active", dataModel.active));
          updateCommand.Parameters.Add(new SqlParameter("@currentLector", currentLector.Id));
          updateCommand.Parameters.Add(new SqlParameter("@name", name));

          updateCommand.ExecuteNonQuery();
        }
        else
        {
          reader.Close();
          string insertSql = $"INSERT INTO StudentLectorDatabaseTable (STUDENTNAME, LECTOREMAIL, ACTIVE) VALUES (@studentName, @lectorEmail, @active)";

          SqlCommand insertCommand = new SqlCommand(insertSql, con);

          insertCommand.Parameters.Add(new SqlParameter("@studentName", dataModel.studentName));
          insertCommand.Parameters.Add(new SqlParameter("@lectorEmail", dataModel.lectorEmail));
          insertCommand.Parameters.Add(new SqlParameter("@active", dataModel.active));

          insertCommand.ExecuteNonQuery();
        }

        con.Close();
      }
    }
  }
}