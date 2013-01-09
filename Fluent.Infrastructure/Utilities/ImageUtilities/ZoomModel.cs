using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Utilities.ImageUtilities
{
    public enum ZoomModel
    {
        [Description("根据画布等比缩放")]
        Ratio = 0,
        [Description("仅向缩小时等比缩放，当图像小于画布时不缩放")]
        RatioOnlySmaller = 1,
        [Description("不缩放直接左上角剪切")]
        NonRatioCuter = 2
    }
}
