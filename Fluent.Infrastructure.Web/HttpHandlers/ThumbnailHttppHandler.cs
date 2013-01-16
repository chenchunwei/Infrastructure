using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using Fluent.Infrastructure.Utilities.ImageUtilities;
using Fluent.Infrastructure.Web.Utilities;

namespace Fluent.Infrastructure.Web.HttpHandlers
{
    public class ThumbnailHttppHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var paramsStr = context.Request["params"];
            //判断图片是否存在
            var physicalPath = context.Request.PhysicalPath;

            if (string.IsNullOrEmpty(paramsStr))
            {
                context.Response.WriteFile(context.Request.PhysicalPath);
                return;
            }
            var thumbProperty = DefaultThumbnailNameProvider.GetThumbnailPropertyFromParamExpression(paramsStr);
            if (thumbProperty == null)
            {
                context.Response.WriteFile(context.Request.PhysicalPath);
                return;
            }
            var defaultThumbnailNameProvider = new DefaultThumbnailNameProvider();
            var originalFileInfo = new FileStoreInfo(physicalPath);
            var thumbImageFileInfo = defaultThumbnailNameProvider.GetThumbnailFileInfo(originalFileInfo, thumbProperty);
            if (!File.Exists(thumbImageFileInfo.FullFileName))
            {
                ImageUtility.Thumbnail(originalFileInfo, thumbProperty, defaultThumbnailNameProvider);
            }
            context.Response.ClearHeaders();
            context.Response.ContentType = "image/jpeg";
            context.Response.Headers.Add("Content-Type","image/jpeg");
            //context.Response.AddHeader("Content-Disposition", "attachment;filename=" + originalFileInfo.FileNameWithExtension); 
            context.Response.WriteFile(thumbImageFileInfo.FullFileName);
        }
    }
}
