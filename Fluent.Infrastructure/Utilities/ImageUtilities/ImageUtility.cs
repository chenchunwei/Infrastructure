using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Utilities.ImageUtilities
{
    /// <summary>
    /// 生成图片缩略图
    /// </summary>
    public class ImageUtility
    {
        public static void Thumbnail(FileStoreInfo originalImageFileInfo, ThumbnailProperty thumbnailProperty, IThumbnailNameProvider thumbnailWriteProvider = null, params ThumbnailProperty[] thumbnailProperties)
        {
            var defaultThumbnailWriteProvider = thumbnailWriteProvider;
            if (originalImageFileInfo == null)
                throw new ArgumentNullException("originalImageFileInfo");
            if (string.IsNullOrEmpty(originalImageFileInfo.FileNameWithExtension))
                throw new ArgumentException("originalImageFileInfo.FileNameWithExtension不能为空");
            if (thumbnailProperty == null)
                throw new ArgumentNullException("thumbnailProperty");
            if (!File.Exists(originalImageFileInfo.FullFileName))
                throw new FileNotFoundException(string.Format("路径：{0}的不存在", originalImageFileInfo.FullFileName));
            if (thumbnailWriteProvider == null)
                defaultThumbnailWriteProvider = new DefaultThumbnailNameProvider();
            var unionThumbnailPropertyies = new[] { thumbnailProperty };
            if (thumbnailProperties != null)
            {
                unionThumbnailPropertyies = unionThumbnailPropertyies.Union(thumbnailProperties).ToArray();
            }
            var originalImage = System.Drawing.Image.FromFile(originalImageFileInfo.FullFileName);
            foreach (var property in unionThumbnailPropertyies)
            {
                var fileInfo = defaultThumbnailWriteProvider.GetThumbnailFileInfo(originalImageFileInfo, thumbnailProperty);
                var thumbnailImage = CreateThumbnailImage(originalImage, property);
                thumbnailImage.Save(fileInfo.FullFileName);
            }
        }

        private static System.Drawing.Image CreateThumbnailImage(Image originalImage, ThumbnailProperty thumbnailProperty)
        {
            var width = originalImage.Width;
            var height = originalImage.Height;
            var targetWidth = thumbnailProperty.Width;
            var targetHeight = thumbnailProperty.Height;
            int newWidth, newHeight;
            Tuple<int, int> size;
            switch (thumbnailProperty.ZoomModel)
            {
                case ZoomModel.Ratio:
                    size = CalculateWidthAndHeight(targetWidth, targetHeight, width, height, true);
                    newWidth = size.Item1;
                    newHeight = size.Item2;
                    break;
                case ZoomModel.RatioOnlySmaller:
                    size = CalculateWidthAndHeight(targetWidth, targetHeight, width, height, false);
                    newWidth = size.Item1;
                    newHeight = size.Item2;
                    break;
                default:
                    newWidth = thumbnailProperty.Width;
                    newHeight = thumbnailProperty.Height;
                    break;
            }
            newWidth = newWidth > targetWidth ? targetWidth : newWidth;
            newHeight = newHeight > targetHeight ? targetHeight : newHeight;
            if (thumbnailProperty.GraphFillModel == GraphFillModel.NonFill)
            {
                targetWidth = newWidth;
                targetHeight = newHeight;
            }
            var finalImage = new System.Drawing.Bitmap(targetWidth, targetHeight);
            var graphic = System.Drawing.Graphics.FromImage(finalImage);
            graphic.FillRectangle(new System.Drawing.SolidBrush(thumbnailProperty.Color), new System.Drawing.Rectangle(0, 0, targetWidth, targetHeight));
            var pasteX = (targetWidth - newWidth) / 2;
            var pasteY = (targetHeight - newHeight) / 2;
            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphic.DrawImage(originalImage, pasteX, pasteY, newWidth, newHeight);
            return finalImage;
        }

        public static void Thumbnail(Stream stream, string thumbnailPath, ThumbnailProperty thumbnailProperty, params ThumbnailProperty[] thumbnailProperties)
        {

        }

        private static Tuple<int, int> CalculateWidthAndHeight(int targetWidth, int targetHeight, int originalWith, int originalHeight, bool isZoomOut)
        {
            var newWidth = 0;
            var newHeight = 0;

            if (targetWidth > originalWith && targetHeight > originalHeight && !isZoomOut)
            {
                newWidth = originalWith;
                newHeight = originalHeight;
            }
            else
            {
                double targetRatio = targetWidth / (float)targetHeight;
                var imageRatio = originalWith / (float)originalHeight;

                if (targetRatio > imageRatio)
                {
                    newHeight = targetHeight;
                    newWidth = (int)Math.Floor(imageRatio * targetHeight);
                }
                else
                {
                    newHeight = (int)Math.Floor(targetWidth / imageRatio);
                    newWidth = targetWidth;
                }
            }
            return new Tuple<int, int>(newWidth, newHeight);
        }
    }
}
