using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Utilities.ImageUtilities
{
    public class DefaultThumbnailNameProvider : IThumbnailNameProvider
    {
        public FileStoreInfo GetThumbnailFileInfo(FileStoreInfo originalFileInfo, ThumbnailProperty thumbnailProperty)
        {
            Console.WriteLine(ColorHelper.ColorToHex(thumbnailProperty.Color));
            const string templateOfThumbName = "_thumb_{0}x{1}x{2}x{3}x{4}";
            //原文件名+"_thumb_[0width]x[1height]x[2zoomModeld]x[3fillModel]x[4color]"
            var postfix = string.Format(templateOfThumbName, thumbnailProperty.Width, thumbnailProperty.Height,
                    (int)thumbnailProperty.ZoomModel, (int)thumbnailProperty.GraphFillModel, ColorHelper.ColorToHex(thumbnailProperty.Color));
            var fileStoreInfo = new FileStoreInfo()
            {
                ExtensionName = string.IsNullOrEmpty(thumbnailProperty.ExtensionName)
                    ? originalFileInfo.ExtensionName
                    : thumbnailProperty.ExtensionName,
                FileNameWithoutExtension = originalFileInfo.FileNameWithExtension + postfix,
                FileDirectory = originalFileInfo.FileDirectory
            };
            return fileStoreInfo;
        }

        public static ThumbnailProperty GetThumbnailPropertyFromFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }
            var names = fileName.Split(new[] { "_thumb_" }, StringSplitOptions.RemoveEmptyEntries);
            if (names.Length != 2)
                return null;
            if (names[1].IndexOf('.') < 0)
            {
                return null;
            }
            var indexOfPoint = names[1].IndexOf('.');
            var paramtersStr = names[1].Substring(0, indexOfPoint);
            var extension = names[1].Replace(paramtersStr, "");
            var parameters = paramtersStr.Split(new[] { 'x' }, StringSplitOptions.RemoveEmptyEntries);
            if (parameters.Length != 4)
                return null;
            try
            {
                return new ThumbnailProperty()
                {
                    Color = ColorHelper.HexToColor(parameters[3]),
                    ExtensionName = extension,
                    Width = int.Parse(parameters[0]),
                    Height = int.Parse(parameters[1]),
                    ZoomModel = (ZoomModel)int.Parse(parameters[2])
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ThumbnailProperty GetThumbnailPropertyFromParamExpression(string expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var indexOfPoint = expression.IndexOf('.');
            var paramtersStr = expression.Substring(0, indexOfPoint);
            var extension = expression.Replace(paramtersStr, "");
            var parameters = paramtersStr.Split(new[] { 'x' }, StringSplitOptions.RemoveEmptyEntries);
            if (parameters.Length != 5)
                return null;
            try
            {
                return new ThumbnailProperty()
                {
                    Color = ColorHelper.HexToColor(parameters[3]),
                    ExtensionName = extension,
                    Width = int.Parse(parameters[0]),
                    Height = int.Parse(parameters[1]),
                    ZoomModel = (ZoomModel)int.Parse(parameters[2]),
                    GraphFillModel = (GraphFillModel)int.Parse(parameters[3])
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static FileStoreInfo GetOriginalFileNameFromFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }
            var names = fileName.Split(new[] { "_thumb_" }, StringSplitOptions.RemoveEmptyEntries);
            if (names.Length != 2)
                return new FileStoreInfo(fileName);
            return new FileStoreInfo(names[0]);
        }
    }
}
