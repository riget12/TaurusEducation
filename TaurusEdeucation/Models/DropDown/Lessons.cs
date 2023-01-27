using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using TaurusEdeucation.config;

namespace TaurusEdeucation.Models
{
    public class Lessons
    {
        public string Output { get; set; }

        //seznam možných studií
        public List<SelectListItem> List;

        public Lessons()
        {
            List = new List<SelectListItem>();

            using (SqlConnection con = new SqlConnection(ConnectionStrings.UmbracoDbDSN))
            {
                con.Open();

                string sql = $"select Id, Name"
                           + $"  from Lessons";

                SqlCommand command = new SqlCommand(sql, con);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    List.Add(new SelectListItem() { Text = reader.GetString(1), Value = reader.GetString(0) });
                }
            }
        }
    }
}