using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Utilities.ImageUtilities
{
    public interface IThumbnailNameProvider
    {
        FileStoreInfo GetThumbnailFileInfo(FileStoreInfo originalFileInfo, ThumbnailProperty thumbnailProperty);
    }
}
