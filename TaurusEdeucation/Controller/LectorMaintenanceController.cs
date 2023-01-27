using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TaurusEdeucation.Models;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.WebApi;

namespace TaurusEdeucation.Controller
{
    /// <summary>
    /// TODO dořešit
    /// </summary>
    [Route("api/LectorMaintenance")]
    public class LectorMaintenanceController : UmbracoApiController
    {
        [HttpGet]
        public void Test()
        {

        }

        [AcceptVerbs("POST")]
        public void BanLector(string email)
        {

        }

        [AcceptVerbs("POST")]
        public void UnbanLector(string email)
        {

        }

        [AcceptVerbs("POST")]
        public void DeleteLector(string email)
        {

        }
    }
}