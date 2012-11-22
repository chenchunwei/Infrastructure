using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Web.HttpFiles
{
    public class HttpFileInfo
    {
        public string OriginalFileName { get; set; }
        public string ServerFileName { get; set; }
        public string ServerPath { get; set; }
        public int FileSize { get; set; }
        public string Extension { get; set; }
    }
}
