using System;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Fluent.Infrastructure.Mvc
{
    public class NewtonsoftJsonResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "text/html"; //兼容IE9
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
#pragma warning disable 0618
                response.Write(JsonConvert.SerializeObject(Data));
#pragma warning restore 0618
            }
        }
    }
}
