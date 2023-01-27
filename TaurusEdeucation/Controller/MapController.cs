using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TaurusEdeucation.config;
using TaurusEdeucation.Models;
using Umbraco.Web.WebApi;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace TaurusEdeucation.Controller
{
    [Route("api/map")]
    public class MapController : UmbracoApiController
    {
        [HttpGet]
        public string Test()
        {
            return "Test";
        }

        /// <summary>
        /// Získání seznamu krajů pro mapu
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetKrajeForMap()
        {
            List<KrajViewModel> result = new List<KrajViewModel>();

            using (SqlConnection con = new SqlConnection(ConnectionStrings.UmbracoDbDSN))
            {
                con.Open();

                string sql = $"select Code, Name, Coords"
                           + $"  from kraje as k";

                SqlCommand command = new SqlCommand(sql, con);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string code = reader.GetString(0);
                    string name = reader.GetString(1);
                    string coords = reader.GetString(2);

                    result.Add(new KrajViewModel() { Code = code, Name = name, Coords = coords });
                }
            }

            return new JsonResult()
            {
                Data = JsonConvert.SerializeObject(result),
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        /// <summary>
        /// Získání seznamu Okresů
        /// </summary>
        /// <param name="kod"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetOkresy(string kod)
        {
            if (String.IsNullOrEmpty(kod))
            {
                throw new ArgumentNullException();
            }

            Dictionary<string, string> result = new Dictionary<string, string>();

            using (SqlConnection con = new SqlConnection(ConnectionStrings.UmbracoDbDSN))
            {
                con.Open();

                string sql = $"select Code, Name"
                           + $"  from okresy as o"
                           + $"  where o.Kraj_Id = (select Id from Kraje as k where k.Code = @kod)";

                SqlCommand command = new SqlCommand(sql, con);
                command.Parameters.Add(new SqlParameter("@kod", kod));
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string code = reader.GetString(0);
                    string name = reader.GetString(1);

                    result.Add(code, name);
                }
            }

            return new JsonResult()
            {
                Data = JsonConvert.SerializeObject(result),
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }
    }

    
}