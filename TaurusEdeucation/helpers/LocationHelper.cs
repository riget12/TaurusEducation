using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TaurusEdeucation.config;

namespace TaurusEdeucation.helpers
{
    public static class LocationHelper
    {
        public static string GetKrajName(string code)
        {
            using (SqlConnection con = new SqlConnection(ConnectionStrings.UmbracoDbDSN))
            {
                con.Open();

                string sql = "select top 1 Name from Kraje where Code = @code";
                SqlCommand command = new SqlCommand(sql, con);

                command.Parameters.Add(new SqlParameter("@code", code));

                SqlDataReader reader = command.ExecuteReader();

                string result = "";
                if (reader.Read())
                {
                    result = reader.GetString(0);
                }

                return result;
            }
        }

        public static string[] GetOkresyName(string[] codes)
        {
            List<string> result = new List<string>();

            using (SqlConnection con = new SqlConnection(ConnectionStrings.UmbracoDbDSN))
            {
                con.Open();

                string sql = "select Name from Okresy where Code in (";
                SqlCommand command = new SqlCommand();
                command.Connection = con;

                for (int i = 0; i < codes.Length; i++)
                {
                    sql += $"@code{i},";
                    command.Parameters.Add(new SqlParameter($"@code{i}", codes[i]));
                }

                sql = sql.TrimEnd(',');
                sql += ")";
                command.CommandText = sql;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    result.Add(name);
                }

                return result.ToArray();
            }
        }
    }
}