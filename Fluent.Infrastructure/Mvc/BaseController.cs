using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Fluent.Infrastructure.Mvc
{
    public class BaseController : Controller
    {
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding)
        {
            return new NewtonsoftJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };
        }
    }
}
