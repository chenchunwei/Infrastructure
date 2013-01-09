using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Utilities.ImageUtilities
{
    public class FileStoreInfo
    {
        public FileStoreInfo() { }
        public FileStoreInfo(string fileFullPath)
        {
            FileInfo fileInfo = new FileInfo(fileFullPath);
            ExtensionName = fileInfo.Extension;
            FileNameWithoutExtension = fileInfo.Name.TrimEnd(fileInfo.Extension.ToArray());
            FileDirectory = fileInfo.DirectoryName;
        }

        public string FileDirectory { get; set; }
        public string FileNameWithoutExtension { get; set; }
        public string ExtensionName { get; set; }
        public string FullFileName
        {
            get { return Path.Combine(FileDirectory, FileNameWithoutExtension + ExtensionName); }
        }

        public string FileNameWithExtension
        {
            get { return FileNameWithoutExtension + ExtensionName; }
        }
    }
}
