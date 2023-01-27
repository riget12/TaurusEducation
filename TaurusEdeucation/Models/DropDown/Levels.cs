using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaurusEdeucation.config;

namespace TaurusEdeucation.Models.DropDown
{
    public class Levels
    {
        public string Output { get; set; }

        //seznam možných studií
        public List<SelectListItem> List;

        public Levels()
        {
            List = new List<SelectListItem>();

            using (SqlConnection con = new SqlConnection(ConnectionStrings.UmbracoDbDSN))
            {
                con.Open();

                string sql = $"select Id, Name"
                           + $"  from Levels";

                SqlCommand command = new SqlCommand(sql, con);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    List.Add(new SelectListItem() { Text = reader.GetString(1), Value = reader.GetInt64(0).ToString() });
                }
            }
        }
    }
}