using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
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
    [Route("api/SablonyApi")]
    public class SablonyApiController : UmbracoApiController
    {
        [HttpGet]
        public JsonResult GetSablona(string typSablonyString)
        {
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

            var path = media.GetValue("umbracoFile").ToString();
            path = path.Replace("/", "\\").TrimStart('\\');
            var filePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, path);

            using (var sr = new StreamReader(filePath))
            {
                string content = sr.ReadToEnd();
            }


    //        var fileInfo = new FileInfo(
    //Path.Combine(new DirectoryInfo(HostingEnvironment.ApplicationPhysicalPath).Parent.Name, @"filefolder/myfile.txt"));
            //media.SetValue(Services.ContentTypeBaseServices, "umbracoFile", imageName, stream);

            //media.GetValue(Services.ContentTypeService, )

            return null;
        }
    }
}