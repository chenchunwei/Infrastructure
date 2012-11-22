using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using Fluent.Infrastructure.Mvc;
using System.IO;

namespace Fluent.Infrastructure.Web.HttpFiles
{
    public class FileHelper
    {
        private HttpContext _httpContext;
        private Dictionary<string, IList<HttpFileInfo>> filesDict;
        const int Maxattachsize = 8097152;
        private const string Upext = "txt,rar,zip,jpg,jpeg,gif,png,swf,wmv,avi,wma,mp3,mid";
        public FileHelper()
        {
            filesDict = new Dictionary<string, IList<HttpFileInfo>>();
        }
        public Dictionary<string, IList<HttpFileInfo>> Upload()
        {
            _httpContext = HttpContext.Current;
            var index = 0;
            foreach (string key in _httpContext.Request.Files.Keys)
            {
                var file = _httpContext.Request.Files[index];
                index++;
                if (file.ContentLength <= 0) continue;
                if (!filesDict.ContainsKey(key))
                {
                    filesDict[key] = new List<HttpFileInfo>();
                }
                filesDict[key].Add(SaveAndReturnFileInfo(file));
            }
            return filesDict;
        }

        private HttpFileInfo SaveAndReturnFileInfo(HttpPostedFile postedFile)
        {
            if (postedFile.ContentLength == 0)
                throw new ArgumentException("postedFile大小不能为0");
            var clientFileName = postedFile.FileName;
            var extension = GetFileExt(clientFileName);
            if (postedFile.ContentLength > Maxattachsize) { }
            if (("," + Upext + ",").IndexOf("," + extension + ",", System.StringComparison.CurrentCultureIgnoreCase) < 0) { }
            var folderTuple = CreateFolderInfos();
            var newFileName = Guid.NewGuid().ToString().Replace("-", "") + "." + extension;
            var fullPath = Path.Combine(folderTuple.Item2, newFileName);
            postedFile.SaveAs(fullPath);
            return new HttpFileInfo()
            {
                Extension = extension,
                FileSize = postedFile.ContentLength,
                OriginalFileName = clientFileName,
                ServerFileName = newFileName,
                ServerPath = folderTuple.Item1,
            };
        }

        private static string GetFileExt(string fullPath)
        {
            return fullPath != "" ? fullPath.Substring(fullPath.LastIndexOf('.') + 1).ToLower() : "";
        }

        private Tuple<string, string> CreateFolderInfos()
        {
            var reletivePath = "Attachments";
            reletivePath = Path.Combine(HostingEnvironment.ApplicationHost.GetVirtualPath(), reletivePath);
            var attachDir = _httpContext.Server.MapPath(reletivePath);
            if (!Directory.Exists(attachDir))
                Directory.CreateDirectory(attachDir);
            return new Tuple<string, string>(reletivePath, attachDir);
        }

        public IEnumerable<HttpFileInfo> GetFileInfosByKey(string key)
        {
            return filesDict.ContainsKey(key) ? filesDict[key] : Enumerable.Empty<HttpFileInfo>();
        }
    }
}
