using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Utilities.ImageUtilities
{
    public class ThumbnailProperty
    {
        public ThumbnailProperty()
        {
            Color = Color.White;
            ZoomModel = ZoomModel.Ratio;
            GraphFillModel = GraphFillModel.Fill;
        }
        public int Width { get; set; }
        public int Height { get; set; }
        public Color Color { get; set; }
        public ZoomModel ZoomModel { get; set; }
        public GraphFillModel GraphFillModel { get; set; }
        /// <summary>
        /// 需要加.***
        /// </summary>
        public string ExtensionName { get; set; }
    }
}
