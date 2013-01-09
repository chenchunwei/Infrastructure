using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Utilities.ImageUtilities
{
    public enum GraphFillModel
    {
        [Description("空白填充")]
        Fill = 0,
        [Description("不填充，自适应")]
        NonFill = 1,
    }
}
