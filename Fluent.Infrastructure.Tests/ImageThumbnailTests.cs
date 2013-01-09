using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Fluent.Infrastructure.Utilities.ImageUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Fluent.Infrastructure.Tests
{
    [TestClass]
    public class ImageThumbnailTests
    {
        [TestMethod]
        public void TestImageThumbnail()
        {

            var originalImageStoreFileInfoFirst = new FileStoreInfo("C:\\按周查看.bmp");
            var originalImageStoreFileInfoSecond = new FileStoreInfo()
            {
                FileNameWithoutExtension = "按周查看",
                FileDirectory = "c:\\",
                ExtensionName = ".bmp"
            };
            var thumbnailPropertyFisrt = new ThumbnailProperty()
            {
                Color = Color.White,
                Width = 200,
                Height = 200
            };
            var thumbnailPropertySecond = new ThumbnailProperty()
            {
                Color = Color.Transparent,
                Width = 940,
                Height = 360
            };
            var thumbnailPropertyThird = DefaultThumbnailNameProvider
                .GetThumbnailPropertyFromFileName("C:\\按周查看.bmp_thumb_800x400x0xoooooo.png");
            var ogrinalFileInfo =
                DefaultThumbnailNameProvider.GetOriginalFileNameFromFileName("C:\\按周查看.bmp_thumb_800x400x0xoooooo.png");
            ImageUtility.Thumbnail(originalImageStoreFileInfoFirst, thumbnailPropertyFisrt);
            ImageUtility.Thumbnail(originalImageStoreFileInfoSecond, thumbnailPropertySecond);
            ImageUtility.Thumbnail(originalImageStoreFileInfoSecond, thumbnailPropertyThird);
        }
    }
}
