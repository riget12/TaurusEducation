using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace TaurusEdeucation.Types
{
  public class JsonHttpStatusResult : JsonResult
  {
    private readonly HttpStatusCode _httpStatusCode;

    public JsonHttpStatusResult(object data, HttpStatusCode httpStatusCode)
    {
      Data = data;
      _httpStatusCode = httpStatusCode;
    }

    public override void ExecuteResult(ControllerContext context)
    {
      context.RequestContext.HttpContext.Response.StatusCode = (int)_httpStatusCode;
      base.ExecuteResult(context);
    }
  }
}