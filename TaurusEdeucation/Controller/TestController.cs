using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaurusEdeucation.Types;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.WebApi;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace TaurusEdeucation.Controller
{
    [Route("api/TestApi")]
    public class TestController : UmbracoApiController
    {
        [HttpGet]
        public JsonResult GetSablona()
        {
            string typSablonyString = null;
            SablonyTypes typSablony = (SablonyTypes)Enum.Parse(typeof(SablonyTypes), typSablonyString);
            IMediaService mediaService = Services.MediaService;
            IMedia media = null;

            switch (typSablony)
            {
                case SablonyTypes.UspesnaRegistraceLektora:
                    media = mediaService.GetById(1553);
                    break;

                default:
                    break;
            }

            var a = media.GetValue("umbracoFile");
            //media.SetValue(Services.ContentTypeBaseServices, "umbracoFile", imageName, stream);

            //media.GetValue(Services.ContentTypeService, )

            return null;
        }
    }
}